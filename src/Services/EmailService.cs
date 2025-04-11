using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DotNetEnv;

namespace RpaConsultaAtivosB3.src.Services
{
    public class EmailService
    {
        public void EnviarPlanilhaPorEmail(string caminhoArquivoExcel)
        {
            // Carrega variáveis do .env
            Env.Load();
            var remetente = Environment.GetEnvironmentVariable("EMAIL_SUPORTE");
            var senha = Environment.GetEnvironmentVariable("SENHA_EMAIL");

            string destinatario = "testes0101@gmail.com";
            string assunto = "Relatório de Cotações";
            string corpo = "Segue em anexo a planilha com os dados dos ativos.";


            try
            {
                // 2. Verifica se o arquivo existe
                if (!File.Exists(caminhoArquivoExcel))
                {
                    Console.WriteLine("❌ Arquivo não encontrado: " + caminhoArquivoExcel);
                    return;
                }

                // 3. Cria o objeto de e-mail
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(remetente),
                    Subject = assunto,
                    Body = corpo
                };

                // 4. Adiciona o destinatário
                mail.To.Add(destinatario);

                // 5. Adiciona o anexo
                mail.Attachments.Add(new Attachment(caminhoArquivoExcel));

                // 6. Configuração do servidor SMTP do Gmail
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(remetente, senha);
                    smtp.EnableSsl = true;

                    // 7. Envia o e-mail
                    smtp.Send(mail);
                    Console.WriteLine("📧 E-mail enviado com sucesso!");
                }
            }
            catch (SmtpException ex)
            {
                Console.WriteLine("❌ Erro SMTP ao enviar o e-mail:");
                Console.WriteLine(ex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Erro inesperado:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}