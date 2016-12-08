using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace DxRemember.Utils
{
    public class RegUtils
    {
        public List<string> ExtractAndFormatData(string content)
        {
            content = content.Replace('\"', ' ');

            string name = string.Empty;
            string phone = string.Empty;
            string email = string.Empty;

            string namePattern = @"[가-힣| *]{2,4}|[a-zA-Z]{2,10}\s[a-zA-Z]{2,10}";
            string emailPattern = @"([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)";

            Match match = Regex.Match(content, namePattern);
            if (match.Success)
            {
                name = match.Captures[0].Value;
            }

            match = Regex.Match(content, emailPattern);
            if (match.Success)
            {
                email = match.Captures[0].Value;
            }

            int iPos = content.IndexOf("text : 010");
            iPos = content.IndexOf("text", iPos + 2);
            string fnum = content.Substring(iPos + 7, 4);
            iPos = content.IndexOf("text", iPos + 2);
            string lnum = content.Substring(iPos + 7, 4);
            phone = string.Format(@"010-{0}-{1}", fnum, lnum);

            return new List<string>()
                    {
                        name,
                        phone,
                        email
                    };
        }

        public bool IsValidPhone(string Phone)
        {
            try
            {
                if (string.IsNullOrEmpty(Phone))
                    return false;
                var r = new Regex(@"\(?\d{3}\)?-? *\d{3,4}-? *-?\d{4}");
                return r.IsMatch(Phone);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsValidName(string Phone)
        {
            try
            {
                if (string.IsNullOrEmpty(Phone))
                    return false;
                // 한글이름 : [가-힣| *]{2,4}
                // 영문이름 : [a-zA-Z]{2,10}\s[a-zA-Z]{2,10}
                var r = new Regex(@"^[가-힣| *]{2,4}|[a-zA-Z]{2,10}\s[a-zA-Z]{2,10}$");
                return r.IsMatch(Phone);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}