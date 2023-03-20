using System;
using System.Collections.Generic;

namespace TrackingServices.Domain
{
    public partial class UserPrinting
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime DateSent { get; set; }
        public bool IsDownloaded { get; set; }
        public DateTime? TimeDownloaded { get; set; }
        public string AdminName { get; set; } = null!;
        public string? PcName { get; set; }
        public bool? IsDone { get; set; }
        public int? FilesId { get; set; }
        public bool IsUploadToTrackOrders { get; set; }
        public bool? IsSendQueue { get; set; }

        public virtual FilesTbl? Files { get; set; }
    }
}
