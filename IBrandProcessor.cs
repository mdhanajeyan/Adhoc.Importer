using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adhoc.Importer
{
    public interface IBrandProcessor
    {
        void Process(DataTable dt);
    }
}
