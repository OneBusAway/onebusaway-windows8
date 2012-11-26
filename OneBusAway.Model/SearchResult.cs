using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Model
{
    public class SearchResult : BindableBase
    {
        private string title;
        private string subTitle;
        private Type resultType;

        public SearchResult(Type _resultType)
        {
            resultType = _resultType;
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                SetProperty(ref this.title, value);
            }
        }

        public string SubTitle
        {
            get
            {
                return this.subTitle;
            }
            set
            {
                SetProperty(ref this.subTitle, title);
            }
        }

        public Type ResultType
        {
            get
            {
                return resultType;
            }
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
