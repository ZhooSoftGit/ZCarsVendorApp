using System.Net.Http.Headers;
using ZCarsDriver.Core;
using ZCarsDriver.Services.Contracts;
using ZhooCars.Common;
using ZhooCars.Model.DTOs;
using ZhooCars.Model.Request;
using ZhooCars.Services;
using ZhooSoft.Core;
using ZhooSoft.ServiceBase;

namespace ZCarsDriver.Services.Services
{
    public class DocumentService : IDocumentService
    {
        #region Fields

        private readonly IZhooCarService _apiService;

        #endregion

        #region Constructors

        public DocumentService(IZhooCarService apiService)
        {
            _apiService = apiService;
        }

        #endregion

        #region Methods


        public void UpdateLocalData(List<DocumentDto> response)
        {
            var existingDocs = GenericPreferenceService.Instance.Get<List<DocumentDto>>(GlobalConstants.UserDocs);

            if (existingDocs == null)
            {
                existingDocs = new List<DocumentDto>();
            }

            foreach (var newDoc in response)
            {
                var match = existingDocs.FirstOrDefault(d => d.DocumentTypeId == newDoc.DocumentTypeId);

                if (match != null)
                {
                    match.DocumentUrl = newDoc.DocumentUrl;
                    match.ApprovalStatus = ApprovalStatus.Submitted;
                    match.UpdatedAt = newDoc.UpdatedAt;
                }
                else
                {
                    existingDocs.Add(new DocumentDto
                    {
                        DocumentUrl = newDoc.DocumentUrl,
                        CreatedAt = DateTime.Now,
                        DocumentNo = newDoc.DocumentNo,
                        ApprovalStatus = ApprovalStatus.Submitted,
                        DocumentId = newDoc.DocumentId,
                        DocumentTypeId = newDoc.DocumentTypeId
                    });
                }
            }

            GenericPreferenceService.Instance.Set(GlobalConstants.UserDocs, existingDocs);
        }

        public async Task<bool> UploadToCDNAsync(string uploadUrl, Stream fileStream, string contentType)
        {
            using var client = new HttpClient();
            var content = new StreamContent(fileStream);
            content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            var response = await client.PutAsync(uploadUrl, content);

            var str = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode;
        }

        public async Task<ApiResponse<DocumentDto>> UploadDocument(DocumentUploadRequest request)
        {
            try
            {
                var result = await _apiService.PostAsync<DocumentDto>($"{ApiConstants.BaseUrl}{ApiConstants.DocumentUploadDoc}", request.UploadDocumentRequest);

                if (result.IsSuccess && result.Data != null)
                {
                    var cdnresult = await UploadDocToCDN(request, result.Data);

                    if (cdnresult)
                    {
                        UpdateLocalData([result.Data]);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ApiResponse<List<DocumentDto>>> UploadDocuments(List<DocumentUploadRequest> uploadDocumentRequests)
        {
            try
            {
                var result = await _apiService.PostAsync<List<DocumentDto>>($"{ApiConstants.BaseUrl}{ApiConstants.DocumentUploadDocs}", uploadDocumentRequests.Select(x => x.UploadDocumentRequest).ToList());

                if (result.IsSuccess && result.Data != null)
                {
                    UpdateLocalData(result.Data);

                    foreach (var item in uploadDocumentRequests)
                    {
                        var outputdata = result.Data.FirstOrDefault(x => x.DocumentTypeId == item.UploadDocumentRequest.DocumentType);
                        if (outputdata != null)
                        {
                            await UploadDocToCDN(item, outputdata);
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<bool> UploadDocToCDN(DocumentUploadRequest request, DocumentDto documentDto)
        {
            // Launch CDN upload in background
            try
            {
                using var stream = request.DocStream;
                var uploadSuccess = await UploadToCDNAsync(documentDto.SignedUrlForUpload, stream, request.UploadDocumentRequest.ContentType);

                return uploadSuccess;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<ApiResponse<List<DocumentDto>>> GetUserDocuments()
        {
            var url = $"{ApiConstants.BaseUrl}{ApiConstants.DocumentGetDocuments}";
            var result = await _apiService.GetAsync<List<DocumentDto>>(url);
            if (result.IsSuccess && result.Data != null)
            {
                UpdateLocalData(result.Data);
            }

            return result;
        }

        #endregion
    }
}
