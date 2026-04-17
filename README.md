# inclusao-diversidade-dotnet

[![Build Status](https://github.com/seu-usuario/inclusao-diversidade-dotnet/actions/workflows/pipeline.yml/badge.svg)](https://github.com/seu-usuario/inclusao-diversidade-dotnet/actions)
[![.NET 8](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![Docker](https://img.shields.io/badge/Docker-24.x-blue)](https://www.docker.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Sobre o projeto

Sistema ESG (Environmental, Social, Governance) focado no pilar Social - Inclusão e Diversidade. Esta é uma API REST desenvolvida em C# com ASP.NET Core, utilizando banco de dados relacional Oracle, voltada para o monitoramento e gestão de indicadores de diversidade em organizações.

O sistema permite:
- Cadastro e gerenciamento de colaboradores com dados demográficos para análise de diversidade
- Registro e acompanhamento de treinamentos sobre diversidade e inclusão
- Controle de participação em treinamentos por colaborador
- Geração de relatórios de diversidade com métricas e indicadores
- Dashboard com visão geral dos indicadores de inclusão

## Como executar localmente com Docker

### Pré-requisitos

- Docker >= 24.x
- Docker Compose >= 2.x
- Git

### Passo a passo

```bash
# 1. Clone o repositório
git clone https://github.com/seu-usuario/inclusao-diversidade-dotnet.git

# 2. Acesse o diretório do projeto
cd inclusao-diversidade-dotnet

# 3. Copie o arquivo de exemplo de variáveis de ambiente
cp .env.example .env

# 4. Edite o arquivo .env com suas configurações
# Verifique as variáveis na seção abaixo

# 5. Inicie os containers
docker compose up --build -d

# 6. Acesse a aplicação
# Swagger: http://localhost:8080/swagger
# API: http://localhost:8080

# 7. Verificar os logs da aplicação
docker compose logs -f app

# 8. Parar os containers
docker compose down
```

### Variáveis de ambiente disponíveis no .env.example

| Variável | Descrição | Valor Padrão |
|----------|-----------|--------------|
| `ASPNETCORE_ENVIRONMENT` | Ambiente de execução (Development, Staging, Production) | Development |
| `DB_HOST` | Host do banco de dados | localhost |
| `DB_PORT` | Porta do banco de dados | 1521 |
| `DB_NAME` | Nome do banco de dados | ORCL |
| `DB_USER` | Usuário do banco de dados | system |
| `DB_PASSWORD` | Senha do banco de dados | oracle |
| `JWT_SECRET_KEY` | Chave secreta para tokens JWT | (chave segura) |
| `JWT_ISSUER` | Emissor do token JWT | inclusao-diversidade-api |
| `JWT_AUDIENCE` | Audiência do token JWT | inclusao-diversidade-app |
| `PORT` | Porta da aplicação | 8080 |

## Pipeline CI/CD

### Ferramenta utilizada
GitHub Actions

### Arquivo de configuração
`.github/workflows/pipeline.yml`

### Descrição das etapas

```yaml
name: CI/CD Pipeline

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  # Job 1: Build and Test
  build-and-test:
    runs-on: ubuntu-latest
    steps:
      - name: Checkout
        uses: actions/checkout@v4
      
      - name: Setup .NET 8 SDK
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'
      
      - name: Restore dependencies
        run: dotnet restore
      
      - name: Build
        run: dotnet build --configuration Release
      
      - name: Run tests
        run: dotnet test --configuration Release

  # Job 2: Deploy Staging
  deploy-staging:
    needs: build-and-test
    runs-on: ubuntu-latest
    environment: staging
    steps:
      - name: Build Docker image
        run: |
          docker build -t ghcr.io/${{ github.repository }}:staging .
      
      - name: Push to GHCR
        run: |
          echo "${{ secrets.GITHUB_TOKEN }}" | docker login ghcr.io -u ${{ github.actor }} --password-stdin
          docker push ghcr.io/${{ github.repository }}:staging
      
      - name: Deploy to staging
        run: echo "Deploying to staging environment..."

  # Job 3: Deploy Production (requer aprovação manual)
  deploy-production:
    needs: deploy-staging
    runs-on: ubuntu-latest
    environment: production
    steps:
      - name: Build Docker image
        run: |
          docker build -t ghcr.io/${{ github.repository }}:latest .
      
      - name: Push to GHCR
        run: |
          echo "${{ secrets.GITHUB_TOKEN }}" | docker login ghcr.io -u ${{ github.actor }} --password-stdin
          docker push ghcr.io/${{ github.repository }}:latest
      
      - name: Deploy to production
        run: echo "Deploying to production environment..."
```

### Diagrama textual do fluxo

```
push/PR → build-and-test → deploy-staging → [aprovação manual] → deploy-production
                            ↓
                      Ambiente Staging
                                        ↓
                              Produção (após aprovação)
```

## Containerização

### Dockerfile (multi-stage build)

```dockerfile
# Stage 1: Build - Compila a aplicação
# Usamos a imagem SDK do .NET 8 para compilar o projeto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Define o diretório de trabalho como /src
WORKDIR /src

# Copia o arquivo de projeto primeiro para otimizar o cache do Docker
COPY ["Fiap.Api.InclusaoDiversidadeEmpresas.csproj", "./"]

# Restaura as dependências do projeto
RUN dotnet restore "./Fiap.Api.InclusaoDiversidadeEmpresas.csproj"

# Copia todo o código fonte para a imagem
COPY . .

# Define o diretório de trabalho para a raíz do projeto
WORKDIR "/src/."

# Compila o projeto em modo Release e gera os artefatos em /app/build
RUN dotnet build "./Fiap.Api.InclusaoDiversidadeEmpresas.csproj" -c Release -o /app/build

# Stage 2: Publish - Gera a aplicação final otimizada para publicação
FROM build AS publish

# Publica a aplicação em modo Release gerando artefatos em /app/publish
RUN dotnet publish "./Fiap.Api.InclusaoDiversidadeEmpresas.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime - Imagem final que será executada
# Usamos apenas o runtime ASP.NET (imagem mais leve que o SDK)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Define o diretório de trabalho como /app
WORKDIR /app

# Copia os artefatos publicados do stage anterior para a imagem final
COPY --from=publish /app/publish .

# Define o comando de inicialização do container
ENTRYPOINT ["dotnet", "Fiap.Api.InclusaoDiversidadeEmpresas.dll"]
```

### docker-compose.yml

```yaml
version: '3.8'

services:
  # Serviço da aplicação ASP.NET Core
  app:
    image: inclusao-diversidade-dotnet:latest
    container_name: app-inclusao-diversidade
    build:
      context: ./Fiap.Api.InclusaoDiversidadeEmpresas
      dockerfile: Dockerfile
    ports:
      - "8080:8080"
    env_file:
      - .env
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - DB_HOST=db
      - DB_PORT=1521
      - DB_NAME=ORCL
    depends_on:
      db:
        condition: service_healthy
    networks:
      - inclusao-network
    restart: unless-stopped

  # Serviço do banco de dados Oracle
  db:
    image: container-registry.oracle.com/database/express:21.3.0-xe
    container_name: db-inclusao-diversidade
    environment:
      - ORACLE_PASSWORD=oracle
      - ORACLE_CHARACTERSET=AL32UTF8
    ports:
      - "1521:1521"
      - "5500:5500"
    volumes:
      - oracle-data:/opt/oracle/oradata
    networks:
      - inclusao-network
    healthcheck:
      test: ["CMD", "healthcheck.sh"]
      interval: 30s
      timeout: 10s
      retries: 5
      start_period: 60s
    restart: unless-stopped

# Rede interna para comunicação entre serviços
networks:
  inclusao-network:
    driver: bridge

# Volumes nomeados para persistência de dados
volumes:
  oracle-data:
    driver: local
```

### Estratégias adotadas

- **Imagem leve**: Utilização do `aspnet:8.0` runtime (mais leve que o SDK completo)
- **Multi-stage build**: Separação em stages para otimizar o tamanho da imagem final
- **.dockerignore**: Exclusão de diretórios desnecessários (`bin/`, `obj/`, `.env`)
- **Variáveis externalizadas**: Todas as configurações via variáveis de ambiente
- **Healthcheck**: Verificação de saúde do banco de dados antes de iniciar a aplicação
- **depends_on com condition**: Garante que o banco esteja saudável antes de iniciar a app

## Prints do funcionamento

- [ ] Pipeline rodando no GitHub Actions — jobs build, test e deploy
- [ ] Ambiente staging funcionando (ex: http://localhost:8081/swagger)
- [ ] Ambiente produção funcionando (ex: http://localhost:8080/swagger)
- [ ] Saída do comando: `docker compose ps`
- [ ] Logs da aplicação iniciando: `docker compose logs -f app`
- [ ] Endpoint da API respondendo (ex: GET /api/indicadores via Swagger ou curl)

## Tecnologias utilizadas

| Tecnologia | Versão | Finalidade |
|------------|--------|------------|
| C# | 12 | Linguagem principal |
| ASP.NET Core | 8.0 | Framework Web API |
| Entity Framework Core | 8.x | ORM e migrações |
| Oracle Database | 21c | Banco de dados relacional |
| xUnit | 2.x | Testes unitários |
| Docker | 24.x | Containerização |
| Docker Compose | 2.x | Orquestração local |
| GitHub Actions | — | Pipeline CI/CD |
| GitHub Container Registry | — | Registry de imagens |
| Swagger/OpenAPI | — | Documentação da API |
| JWT | — | Autenticação e autorização |
| AutoMapper | — | Mapeamento de objetos |
| Git | — | Controle de versão |

## Checklist de entrega

| Item | Status |
|------|--------|
| Projeto compactado em .zip com estrutura organizada | ☐ |
| Dockerfile funcional | ☑ |
| docker-compose.yml ou arquivos Kubernetes | ☑ |
| Pipeline com etapas de build, teste e deploy | ☐ |
| README.md com instruções e prints | ☑ |
| Documentação técnica com evidências (PDF ou PPT) | ☐ |
| Deploy realizado nos ambientes staging e produção | ☐ |

## Estrutura do projeto

```
inclusao-diversidade-dotnet/
├── Dockerfile
├── docker-compose.yml
├── .env.example
├── .dockerignore
├── README.md
├── Fiap.Api.InclusaoDiversidadeEmpresas/
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── ColaboradorController.cs
│   │   ├── ParticipacaoEmTreinamentoController .cs
│   │   ├── RelatoriosController.cs
│   │   └── TreinamentoController.cs
│   ├── Models/
│   │   ├── ColaboradorModel.cs
│   │   ├── ParticipacaoEmTreinamentoModel.cs
│   │   ├── QueryParameters.cs
│   │   ├── RelatorioDeDiversidadeModel.cs
│   │   └── TreinamentoModel.cs
│   ├── Services/
│   │   ├── AuthService.cs
│   │   ├── ColaboradorService.cs
│   │   ├── IAuthService.cs
│   │   ├── IColaboradorService.cs
│   │   ├── IParticipacaoEmTreinamentoService.cs
│   │   ├── IRelatorioService.cs
│   │   ├── ITreinamentoService.cs
│   │   ├── ParticipacaoEmTreinamentoService.cs
│   │   ├── RelatorioService.cs
│   │   └── TreinamentoService.cs
│   ├── ViewModel/
│   │   ├── ColaboradorFormViewModel.cs
│   │   ├── ColaboradorListaViewModel.cs
│   │   ├── DashboardDiversidadeViewModel.cs
│   │   ├── ErrorViewModel.cs
│   │   ├── LoginModel.cs
│   │   ├── PagedResultViewModel.cs
│   │   ├── ParticipacaoPaginacaoViewModel.cs
│   │   ├── ParticipacaoViewModel.cs
│   │   ├── TreinamentoPaginacaoViewModel.cs
│   │   └── TreinamentoViewModel.cs
│   ├── Data/
│   │   └── DatabaseContext.cs
│   ├── Migrations/
│   ├── Program.cs
│   ├── appsettings.json
│   └── Fiap.Api.InclusaoDiversidadeEmpresas.csproj
├── Fiap.Api.InclusaoDiversidadeEmpresas.testes/
│   ├── ParticipacaoControllerTestes.cs
│   ├── TreinamentoControllerTests.cs
│   ├── RelatoriosControllerTests.cs
│   └── ColaboradoresControllerTests.cs
└── .github/
    └── workflows/
        └── pipeline.yml
```

## Integrantes

| Nome | RM |
|------|----|
| [Nome completo do integrante 1] | [RM do integrante 1] |
| [Nome completo do integrante 2] | [RM do integrante 2] |
| [Nome completo do integrante 3] | [RM do integrante 3] |
| [Nome completo do integrante 4] | [RM do integrante 4] |
| [Nome completo do integrante 5] | [RM do integrante 5] |

---

**Nota**: Este projeto foi desenvolvido como parte da disciplina de Inclusão e Diversidade da FIAP, focusing no pilar Social do framework ESG.