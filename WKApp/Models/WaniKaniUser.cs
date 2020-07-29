using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WKApp.Models
{

    public class WaniKaniUser
    {
        public string _object { get; set; }
        public string url { get; set; }
        public DateTime data_updated_at { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string id { get; set; }
        public string username { get; set; }
        public int level { get; set; }
        public string profile_url { get; set; }
        public DateTime started_at { get; set; }
        public object current_vacation_started_at { get; set; }
        public Subscription subscription { get; set; }
        public Preferences preferences { get; set; }
    }

    public class Subscription
    {
        public bool active { get; set; }
        public string type { get; set; }
        public int max_level_granted { get; set; }
        public DateTime period_ends_at { get; set; }
    }

    public class Preferences
    {
        public int default_voice_actor_id { get; set; }
        public bool lessons_autoplay_audio { get; set; }
        public int lessons_batch_size { get; set; }
        public string lessons_presentation_order { get; set; }
        public bool reviews_autoplay_audio { get; set; }
        public bool reviews_display_srs_indicator { get; set; }
    }

}
