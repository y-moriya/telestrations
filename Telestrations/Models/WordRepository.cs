using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Telestrations.Models
{
    public interface IWordRepository
    {
        List<string> GetQuestionWords(int count);
    }

    public class WordRepository : IWordRepository
    {
        public List<string> GetQuestionWords(int count)
        {
            throw new NotImplementedException();
        }
    }
}