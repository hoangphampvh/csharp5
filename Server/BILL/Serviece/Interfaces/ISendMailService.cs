using BLL.ViewModel.ModelConfiguration.mailConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Serviece.Interfaces
{
    public interface ISendMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
