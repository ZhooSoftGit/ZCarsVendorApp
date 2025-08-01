using ZhooCars.Common;

namespace ZhooCars.Model.Request
{
    public class UploadDocumentRequest
    {
        public string BucketName { get; set; } = string.Empty;// The target bucket

        public string? ContentType { get; set; }

        public int? DocumentId { get; set; }

        public int ExpirationMinutes { get; set; } = 15;// Expiration time

        public DocumentTypes DocumentType { get; set; }

        public DocumentHttpMethod HttpMethod { get; set; } // "GET" for download, "PUT" for upload

        public string? ObjectKey { get; set; }

        public required string FileName { get; set; }

        public string? DocumentNo { get; set; }

        public int? VehicleId { get; set; }
    }

    public class DocumentUploadRequest
    {
        public UploadDocumentRequest UploadDocumentRequest { get; set; }

        public Stream DocStream { get; set; }
    }

}
