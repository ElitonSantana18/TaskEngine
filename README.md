# TaskEngine

# 📌 Visão Geral

TaskEngine é uma aplicação para o recebimento, armazenamento e processamento assíncrono de tarefas (jobs) em background. O sistema utiliza uma arquitetura escalável, garantindo a execução eficiente e segura das tarefas, com suporte a re-tentativas e controle de concorrência.

## 🚀 Funcionalidades

#### 1️⃣ Recebimento de Tarefas

###### API para criação de novas tarefas para processamento em background.

###### Estrutura flexível para diferentes tipos de tarefas.

###### Armazenamento das tarefas em banco de dados.

#### 2️⃣ Processamento em Segundo Plano

###### Workers processam as tarefas de forma assíncrona via fila.

###### Sistema de re-tentativa em caso de falhas, respeitando um limite máximo de tentativas.

###### Controle de concorrência para evitar conflitos em processamento simultâneo.

#### 3️⃣ Status das Tarefas

###### Registro do status de cada tarefa ("Pendente", "EmProcessamento", "Concluido", "Erro").

###### API para consulta do status das tarefas.

#### 4️⃣ Escalabilidade

###### Suporte para alto volume de tarefas.

###### Uso do serviço de mensageria: RabbitMQ.

#### 5️⃣ Deployment

###### Disponibilização de um Dockerfile para containerização da aplicação.


## 🔧 Tecnologias Utilizadas

###### C# .NET para desenvolvimento da API.

###### MongoDB para armazenamento das tarefas.

###### RabbitMQ para processamento assíncrono.

###### Docker para facilitar a implantação.

## 🔧 Arquitetura
###### DDD - Domain-Driven Design;

## 🔧 Pré-Requisitos
###### [.NET SDK 8.0.0 ou superior](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)
###### [Docker Desktop](https://www.docker.com/products/docker-desktop/)

## 📦 Como Rodar a Aplicação

###### Clone e acesse o repositório - use o comando git clone 
```
git clone https://github.com/ElitonSantana/taskengine.git
cd taskengine
```

###### Suba os containers com Docker Compose:
```
docker-compose up --build
```

###### A [API](http://localhost:5000/swagger/index.html) estará disponível.

## 📜 Autor

### Eliton Alves de Santana
### https://www.linkedin.com/in/eliton-alves-de-santana-69a492198/
