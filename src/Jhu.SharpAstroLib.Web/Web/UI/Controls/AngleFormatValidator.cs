using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Jhu.SharpAstroLib.Coords;

namespace Jhu.SharpAstroLib.Web.UI.Controls
{
    public class AngleFormatValidator : RegularExpressionValidator
    {
        private AngleStyle AngleStyle
        {
            get { return (AngleStyle)(ViewState["AngleStyle"] ?? AngleStyle.Decimal); }
            set { ViewState["AngleStyle"] = value; }
        }

        public AngleFormatValidator()
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            switch (AngleStyle)
            {
                case AngleStyle.Decimal:
                    this.ValidationExpression = Coords.Constants.DecimalFormatString;
                    break;
                case AngleStyle.Hms:
                    this.ValidationExpression = Coords.Constants.HmsFormatString;
                    break;
                case AngleStyle.Dms:
                    this.ValidationExpression = Coords.Constants.DmsFormatString;
                    break;
                default:
                    throw new NotImplementedException();
            }            

            base.OnPreRender(e);
        }
    }
}
