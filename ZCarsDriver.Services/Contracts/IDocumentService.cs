using ZhooCars.Model.DTOs;
using ZhooCars.Model.Request;
using ZhooCars.Model.Response;
using ZhooSoft.ServiceBase;

namespace ZCarsDriver.Services.Contracts
{
    #region Interfaces

    public interface IDocumentService
    {
        #region Methods

        // Upload - Signed URL Based
        Task<ApiResponse<DocumentDto>> UploadDocument(DocumentUploadRequest request);
        Task<ApiResponse<List<DocumentDto>>> UploadDocuments(List<DocumentUploadRequest> uploadDocumentRequests);

        // Retrieval
        Task<ApiResponse<List<DocumentDto>>> GetUserDocuments();

        Task<bool> UploadToCDNAsync(string uploadUrl, Stream fileStream, string contentType);

        void UpdateLocalData(List<DocumentDto> response);

        #endregion
    }

    #endregion

}
