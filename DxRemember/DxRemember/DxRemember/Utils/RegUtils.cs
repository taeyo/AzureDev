using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace DxRemember.Utils
{
    public class RegUtils
    {
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