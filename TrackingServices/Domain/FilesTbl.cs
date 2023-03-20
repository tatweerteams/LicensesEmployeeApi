using System;
using System.Collections.Generic;

namespace TrackingServices.Domain
{
    public partial class FilesTbl
    {
        public FilesTbl()
        {
            UserPrintings = new HashSet<UserPrinting>();
        }

        public int Id { get; set; }
        public string FileName { get; set; } = null!;
        public long? FileNumber { get; set; }

        public virtual ICollection<UserPrinting> UserPrintings { get; set; }
    }
}
