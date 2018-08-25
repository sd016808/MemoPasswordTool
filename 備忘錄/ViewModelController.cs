using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 備忘錄
{
    class ViewModelController
    {
        public string Password { get; set; }
        public BindingList<ViewModel> ViewModelList { get; set; }
        public ViewModelController()
        {
            ViewModelList = new BindingList<ViewModel>();
        }
    }
}
