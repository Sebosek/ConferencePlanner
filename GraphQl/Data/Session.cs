using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConferencePlanner.GraphQl.Data
{
    public class Session
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string? Title { get; set; }

        [StringLength(4000)]
        public string? Abstract { get; set; }

        public DateTimeOffset? StartTime { get; set; }

        public DateTimeOffset? EndTime { get; set; }

        public TimeSpan Duration
        {
            get
            {
                var start = StartTime ?? EndTime ?? DateTimeOffset.MinValue;

                return EndTime?.Subtract(start) ?? TimeSpan.Zero;
            }
        }

        public int? TrackId { get; set; }
        
        public Track? Track { get; set; }

        public ICollection<SessionSpeaker> SessionSpeakers { get; set; } = 
            new List<SessionSpeaker>();

        public ICollection<SessionAttendee> SessionAttendees { get; set; } = 
            new List<SessionAttendee>();
    }
}