using System;
using System.Collections.Generic;
using System.Text;

namespace Expense_Manager
{
    class TransType
    {

        public string TYPE
        {
            get;
            set;
        }

        public char TYPE_IMAGE
        {
            get;
            set;
        }

        public string TYPE_DESCRIPTION
        {
            get;
            set;
        }

        public TransType(string type,char typeImage, string typeDescription)
        {
            this.TYPE = type;
            this.TYPE_IMAGE = typeImage;
            this.TYPE_DESCRIPTION = typeDescription;
        }
    }
}
