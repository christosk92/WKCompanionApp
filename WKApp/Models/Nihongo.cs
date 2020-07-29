using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WKApp.Models
{
    public class Nihongo<T>
    {
        public List<IdData> Ids { get; set; }
        public List<T> Data { get; set; }
    }

    public class Radical
    {

    }

    public class Vocabulary
    {

    }


    public class Kanji
    {
        public int id { get; set; }
        public string url { get; set; }
        public DateTime data_updated_at { get; set; }
        public DataKanji data { get; set; }
    }

    public class DataKanji
    {
        public DateTime created_at { get; set; }
        public int level { get; set; }
        public string slug { get; set; }
        public object hidden_at { get; set; }
        public string document_url { get; set; }
        public string characters { get; set; }
        public Meaning[] meanings { get; set; }
        public Auxiliary_Meanings[] auxiliary_meanings { get; set; }
        public int[] amalgamation_subject_ids { get; set; }
        public string meaning_mnemonic { get; set; }
        public int lesson_position { get; set; }
        public Reading[] readings { get; set; }
        public int[] component_subject_ids { get; set; }
        public object[] visually_similar_subject_ids { get; set; }
        public string meaning_hint { get; set; }
        public string reading_mnemonic { get; set; }
        public string reading_hint { get; set; }
    }

    public class Meaning
    {
        public string meaning { get; set; }
        public bool primary { get; set; }
        public bool accepted_answer { get; set; }
    }

    public class Auxiliary_Meanings
    {
        public string type { get; set; }
        public string meaning { get; set; }
    }

    public class Reading
    {
        public string type { get; set; }
        public bool primary { get; set; }
        public string reading { get; set; }
        public bool accepted_answer { get; set; }
    }

}
