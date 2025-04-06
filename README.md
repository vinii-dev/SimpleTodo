
# SimpleTodo

API desenvolvida para gerenciamento de tarefas (to-do) com autenticação. O projeto utiliza de pardões como Clean Architecture, Result Pattern, Rich Domain e Migrations. \
Grande parte do código está documentada utilizando [XML Documentation Comments](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/), permitindo fácil compreensão do funcionamento de cada parte da aplicação, caso surjam dúvidas durante a leitura ou manutenção do código.
## Documentação da API

### Autenticação

#### Login do usuário

```http
POST /api/Auth/login
```

Autentica um usuário e retorna um token JWT em caso de sucesso.

**Body (JSON):**

```json
{
  "username": "string",
  "password": "string"
}
```

| Código | Significado             |
|--------|--------------------------|
| 200    | Sucesso (retorna token) |
| 401    | Não autorizado           |

---

#### Registro de novo usuário

```http
POST /api/Auth/register
```

Registra um novo usuário.

**Body (JSON):**

```json
{
  "username": "string",
  "password": "string"
}
```

| Código | Significado                   |
|--------|-------------------------------|
| 204    | Registrado com sucesso        |
| 409    | Conflito (usuário já existe)  |

---

### Itens da Lista (TodoItem)

#### Obter lista paginada de itens

```http
GET /api/TodoItem
```

| Parâmetro   | Tipo     | Local     | Descrição                |
|-------------|----------|-----------|---------------------------|
| Page        | integer  | query     | Página atual              |
| PageSize    | integer  | query     | Quantidade por página     |

| Código | Significado |
|--------|-------------|
| 200    | OK          |

---

#### Criar novo item

```http
POST /api/TodoItem
```

**Body (JSON):**

```json
{
  "title": "string",
  "description": "string"
}
```

| Código | Significado |
|--------|-------------|
| 201    | Criado      |
| 400    | Requisição inválida |

---

#### Obter item por ID

```http
GET /api/TodoItem/{id}
```

| Parâmetro | Tipo   | Local | Descrição       |
|-----------|--------|-------|------------------|
| id        | string | path  | ID do item (UUID) |

| Código | Significado     |
|--------|------------------|
| 200    | OK               |
| 404    | Não encontrado   |
| 400    | Requisição inválida |

---

#### Atualizar item (completo)

```http
PUT /api/TodoItem/{id}
```

**Body (JSON):**

```json
{
  "title": "string",
  "description": "string"
}
```

| Código | Significado       |
|--------|--------------------|
| 200    | OK                 |
| 400    | Requisição inválida|
| 404    | Não encontrado     |

---

#### Atualizar item (parcial)

```http
PATCH /api/TodoItem/{id}
```

**Body (JSON):**

```json
{
  "isCompleted": true
}
```

| Código | Significado       |
|--------|--------------------|
| 204    | Sucesso sem conteúdo |
| 400    | Requisição inválida  |
| 404    | Não encontrado       |

---

#### Excluir item

```http
DELETE /api/TodoItem/{id}
```

| Parâmetro | Tipo   | Local | Descrição       |
|-----------|--------|-------|------------------|
| id        | string | path  | ID do item (UUID) |

| Código | Significado       |
|--------|--------------------|
| 204    | Sucesso sem conteúdo |
| 400    | Requisição inválida  |
| 404    | Não encontrado       |


## Rodando localmente

Clone o projeto

```bash
  git clone https://github.com/vinii-dev/simpletodo
```

Entre no diretório do projeto

```bash
  cd SimpleTodo
```

(Opcional) Suba a instância com docker compose. Caso já tenha um banco SQLServer rodando, essa etapa não será necessária.

```
docker compose up -d
```

Atualize o `appsettings.json` com a sua connection string do banco.

Restaure os pacotes NuGet

```bash
  dotnet restore
```

Compile o projeto

```bash
  dotnet build
```

Rode o projeto

```
  dotnet run --project ./SimpleTodo.Api/SimpleTodo.Api.csproj
```



## Detalhes do projeto

### Arquitetura

Este projeto segue os princípios da **Clean Architecture**, com o código dividido em quatro camadas principais:

- **Presentation** - Responsável por lidar com a interface com o usuário ou sistemas externos (ex: controllers, APIs). Essa camada recebe as entradas, valida dados básicos e repassa as chamadas para a camada de Application.

- **Application** - Contém os casos de uso da aplicação. Define o fluxo da lógica de negócio e coordena as chamadas para a camada de Domain. Não possui regras de negócio em si, apenas a orquestração.

- **Domain** - Núcleo da aplicação, onde ficam as entidades e regras de negócio. Essa camada é totalmente isolada e não depende de nenhuma outra.

- **Infrastructure** - Implementa detalhes externos como acesso a banco de dados, serviços externos, e qualquer integração com tecnologia, como geração de token JWT. Essa camada depende das demais, mas nunca o contrário.


### Result Pattern

O principal objetivo é tornar o fluxo de execução mais previsível e seguro, permitindo que os erros sejam tratados de forma explícita. Em vez de lançar exceções em casos de falhas esperadas (como validações), as operações retornam um objeto `Result`, que pode indicar sucesso ou falha, junto com as mensagens ou dados relevantes.

Esse padrão ajuda a:
- Reduzir o custo de exceções desnecessárias
- Melhorar a legibilidade e clareza do código
- Centralizar e padronizar o tratamento de erros

Os erros definidos da aplicação no atual momento da aplicação são:
  - **Auth.InvalidCredentials** - Indica que as credenciais informadas para a autenticação estão incorretas, seja porque o usuário não existe ou porque a senha está incorreta.
  - **Auth.UsernameAlreadyInUse** - Indica que o registro do usuário não foi possível, pois o nome de usuário já foi escolhido.

  - **User.NotFound** - Indica que o usuário informado não foi encontrado.

  - **TodoItem.NotFound** - Indica que o to-do item informado não foi encontrado.

### Middleware para mapear os erros
No `Program.cs` é registrado um middleware que pega esses retornos com erros e os mapeia para um [ProblemDetails](https://datatracker.ietf.org/doc/html/rfc7807). 

### Interceptors
O projeto utiliza **interceptors** para atualizar automaticamente propriedades relacionadas à auditoria das entidades, como `CreatedAt` e `UpdatedAt`.

### Diagrama do banco de dados
[Diagrama do banco de dados](docs/images/database-diagram.png)
## Principais Tecnologias Utilizadas

- [**ErrorOr**](https://github.com/amantinband/error-or) - Biblioteca para auxiliar a aplicação do Result Pattern

- [**EF Core**](https://github.com/dotnet/efcore) - Biblioteca ORM para comunicação com o banco de dados.

- [**Swashbuckle AspNetCore**](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) - Ferramenta para documentação da API

- [**XUnit**](https://xunit.net/) - Framework de testes automatizados

- [**NSubstitute**](https://nsubstitute.github.io/) - Ferramenta para realização de Mocks para testes unitários