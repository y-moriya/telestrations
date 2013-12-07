using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telestrations.Models;

namespace Telestrations.Tests.Models
{
    public class TestWordRepository : IWordRepository
    {
        public List<string> GetQuestionWords(int count)
        {
            var words = new List<string>();
            for (int i = 0; i < count; i++)
            {
                words.Add("hoge" + i.ToString());
            }

            return words;
        }
    }
}
