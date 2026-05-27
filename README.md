# 🛒 ProductsAPI

API RESTful para gerenciamento de produtos com sistema completo de **autenticação e autorização JWT**, desenvolvida em **C# com .NET 10**.

Permite registrar usuários, autenticar via token JWT e gerenciar produtos com controle de acesso baseado em perfis (Admin/User).

---

## 🚀 Tecnologias utilizadas

- [.NET 10](https://dotnet.microsoft.com/) — Framework principal
- [ASP.NET Core Web API](https://learn.microsoft.com/aspnet/core) — Criação da API REST
- [Entity Framework Core](https://learn.microsoft.com/ef/core/) — ORM para acesso ao banco de dados
- [SQLite](https://www.sqlite.org/) — Banco de dados leve, sem necessidade de instalação
- [JWT Bearer](https://jwt.io/) — Autenticação stateless via tokens
- [BCrypt.Net](https://github.com/BcryptNet/bcrypt.net) — Hash seguro de senhas
- [Swagger / OpenAPI](https://swagger.io/) — Documentação interativa da API
- [xUnit](https://xunit.net/) + [Moq](https://github.com/moq/moq4) — Testes unitários

---

## 🏗️ Arquitetura

```
ProductsAPI/
├── Controllers/     → Recebe e responde requisições HTTP
├── Services/        → Regras de negócio e geração de tokens
├── Repositories/    → Acesso ao banco de dados (padrão Repository)
├── Models/          → Entidades do banco (User, Product)
├── DTOs/            → Objetos de transferência de dados
└── Data/            → DbContext e configuração do banco
```

---

## ⚙️ Como executar o projeto

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio Code](https://code.visualstudio.com/)

### Passo a passo

```bash
# Clone o repositório
git clone https://github.com/seu-usuario/Produtos-API.git
cd Produtos-API

# Restaure as dependências
dotnet restore

# Aplique as migrations e crie o banco de dados
dotnet ef database update

# Execute o projeto
dotnet run
```

Acesse a documentação interativa em:
```
http://localhost:5082/swagger
```

---

## 🔐 Como autenticar

### 1. Registre um usuário
```json
POST /api/Auth/register
{
  "name": "Seu Nome",
  "email": "seu@email.com",
  "password": "SuaSenha@123"
}
```

### 2. Faça login e copie o token
```json
POST /api/Auth/login
{
  "email": "seu@email.com",
  "password": "SuaSenha@123"
}
```

Response:
```json
{
  "id": 1,
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "name": "Seu Nome",
  "email": "seu@email.com",
  "role": "User",
  "expiresAt": "2026-05-27T20:00:00Z"
}
```

### 3. Autorize no Swagger
Clique em **Authorize 🔒** e digite:
```
Bearer eyJhbGciOiJIUzI1NiIs...
```

---

## 📋 Endpoints da API

### Auth (público)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/auth/register` | Registra novo usuário |
| POST | `/api/auth/login` | Autentica e retorna token JWT |
| POST | `/api/auth/make-admin/{userId}` | Promove usuário para Admin |

### Products (protegido)

| Método | Endpoint | Perfil | Descrição |
|--------|----------|--------|-----------|
| GET | `/api/products` | User/Admin | Lista todos os produtos |
| GET | `/api/products/{id}` | User/Admin | Busca produto por ID |
| POST | `/api/products` | Admin | Cria novo produto |
| PUT | `/api/products/{id}` | Admin | Atualiza produto |
| DELETE | `/api/products/{id}` | Admin | Remove produto |

---

## 🔑 Controle de acesso

| Ação | User | Admin |
|------|------|-------|
| Listar produtos | ✅ | ✅ |
| Buscar produto | ✅ | ✅ |
| Criar produto | ❌ | ✅ |
| Editar produto | ❌ | ✅ |
| Deletar produto | ❌ | ✅ |

---

## 🧪 Testes

```bash
cd ProductsAPI.Tests
dotnet test
```

```
Passed: 13 — Failed: 0
```

### O que é testado

**AuthService (7 testes)**
- Registro com email duplicado retorna null
- Registro com email novo retorna token JWT
- Login com usuário inexistente retorna null
- Login com senha errada retorna null
- Login com credenciais válidas retorna token
- Promoção de usuário inexistente retorna false
- Promoção de usuário existente altera role para Admin

**ProductService (6 testes)**
- Listagem retorna todos os produtos
- Busca por ID inexistente retorna null
- Criação com preço inválido lança exceção
- Criação com estoque negativo lança exceção
- Criação com dados válidos retorna produto criado
- Exclusão de produto inexistente retorna false

---

## 🧠 Conceitos aplicados

- **JWT Bearer Authentication** — geração e validação de tokens stateless
- **Autorização por Roles** — controle de acesso Admin/User com `[Authorize(Roles)]`
- **BCrypt** — hash seguro de senhas, sem armazenamento em texto puro
- **Arquitetura em camadas** — Controllers → Services → Repositories
- **Padrão Repository** com interfaces para desacoplamento
- **Entity Framework Core** com Code First e Migrations
- **DTOs** para controle do contrato da API
- **Injeção de Dependência** nativa do .NET
- **Programação assíncrona** com async/await
- **Testes unitários** com xUnit, Moq e banco InMemory

---
