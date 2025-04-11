
# 📈 Automação RPA para Consulta de Ativos na B3

Este é o meu **primeiro projeto RPA em C#**, desenvolvido com fins de aprendizado e prática em automação de processos com integração a sites, planilhas e envio de e-mails.

## 🧠 Objetivo

O projeto tem como propósito:

1. Acessar uma planilha Excel e extrair os códigos dos ativos;
2. Acessar o site [https://www.b3.com.br](https://www.b3.com.br);
3. Consultar os ativos listados na planilha;
4. Obter informações detalhadas de cada ativo;
5. Inserir as informações obtidas de volta na planilha;
6. Salvar a planilha atualizada;
7. Enviar a planilha por e-mail com os resultados.

---

## 🗂 Estrutura do Projeto

```
    RPA-B3/
    ├── src/
    │   ├── appsettings.json             # caminhos, timeouts, credenciais…
    │   │
    │   ├── Models/
    │   │   └── AtivoInfo.cs             # DTO com os dados extraídos
    │   │
    │   ├── Builders/                         
    │   │   └── WebDriverBuilder.cs      # configura e retorna IWebDriver
    │   │
    │   ├── Executors/                   # Fazem ações concretas
    │   │   ├── ExcelReader.cs           # lê códigos do Excel
    │   │   └── ExcelWriter.cs           # escreve resultados no Excel 
    │   │
    │   ├── Pages/                       # POM: encapsula UI da B3
    │   │   └── ConsultaAtivoPage.cs     # métodos PesquisarAtivo, ExtrairInformacoes…
    │   │
    │   ├── Orchestrators/               # Coordena todo o fluxo
    │   │   └── AutomationRunnerOrchestrator.cs  # lê, executa página, grava, salva
    │   │
    │   ├── Services/                    # Serviços genéricos (ex: ExcelService unindo reader+writer)
    │   │   └── ExcelService.cs
    │   │
    │   ├── Utils/                       # Helpers cross-cutting (logging, error handling…)
    │   │   └── Log/
    │   │       ├── logs/                # Pasta de arquivos de log
    │   │       └── Logger.cs            # Classe de log
    │   │
    │   └── Program.cs                   # Entry point (pode usar Generic Host e DI)
    │
    ├── appsettings.json                               
    └── RpaConsultaAtivosB3.sln          # Solução .NET
```

---

## 🚀 Como Executar

1. **Clone este repositório:**
   ```bash
   git clone <URL-do-repositório>
   ```

2. **Instale o WebDriver compatível com a versão do seu navegador Chrome** e salve-o na pasta:
   ```
   C:\WebDrivers\chromedriver
   ```

3. **Ajuste o caminho do WebDriver** no arquivo `WebDriverBuilder.cs` se necessário.

4. **Crie um arquivo `.env` na raiz do projeto** com as seguintes variáveis:
   ```env
   EMAIL_SUPORTE=seuEmail@gmail.com
   SENHA_EMAIL=suaSenhaDeApp
   ```

   > ⚠️ Recomenda-se utilizar uma senha de aplicativo (App Password) gerada pelo Google, em vez da sua senha principal.

5. **Configure o destinatário do e-mail:**  
   Na classe `EmailService.cs`, defina o valor da variável `destinatario` com o e-mail que deverá receber a planilha atualizada.

6. **Compile o projeto:**
   ```bash
   dotnet build
   ```

7. **Execute o projeto:**
   ```bash
   dotnet run
   ```

---

## 🛠 Tecnologias Utilizadas

- **C#**
- **.NET 8**
- **EPPlus** — manipulação de arquivos Excel
- **Selenium WebDriver** — automação de navegador
- **System.Net.Mail** — envio de e-mails
- **DotNetEnv** — leitura de variáveis de ambiente

---

## ✅ Funcionalidades

- Leitura e escrita de planilhas Excel
- Automação de consultas no site da B3
- Coleta de dados financeiros de ativos
- Envio automático de e-mail com a planilha atualizada
- Configuração por meio de variáveis de ambiente

---

> Projeto desenvolvido com fins educacionais e de aprendizado pessoal.
