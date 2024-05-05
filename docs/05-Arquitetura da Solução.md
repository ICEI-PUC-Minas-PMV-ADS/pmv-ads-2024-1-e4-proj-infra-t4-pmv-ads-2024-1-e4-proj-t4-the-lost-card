# Arquitetura da Solução

![Arquitetura da Solução](img/arquiteturaSolucao.drawio.png)

## Diagrama de Classes

![Diagrama de classes](img/DiagramaClasses_lostcards.drawio.png)

## Modelo ER

![Schema de gameroom](img/Modelo_ER_Lost_Cards-Gameroom.drawio.png)
![Schema de player](img/Modelo_ER_Lost_Cards-Player.drawio.png)

## Esquema Relacional

### Documento da GameRoom

```json
{
  "$schema": "http://json-schema.org/draft-04/schema#",
  "type": "object",
  "properties": {
    "IsInviteOnly": {
      "type": "boolean"
    },
    "Name": {
      "type": "string"
    },
    "AdminId": {
      "type": "string"
    },
    "Players": {
      "type": "array",
      "items": [
        {
          "type": "object",
          "properties": {
            "PlayerId": {
              "type": "string"
            },
            "ConnectionId": {
              "type": "string"
            }
          },
          "required": [
            "PlayerId",
            "ConnectionId"
          ]
        }
      ]
    },
    "GameInfo": {
      "type": "object",
      "properties": {
        "CurrentLevel": {
          "type": "integer"
        },
        "EncounterInfo": {
          "type": "object",
          "properties": {
            "MonsterMaxLife": {
              "type": "integer"
            },
            "MonsterLife": {
              "type": "integer"
            },
            "MonsterGameClassId": {
              "type": "string"
            },
            "PlayersInfo": {
              "type": "array",
              "items": [
                {
                  "type": "object",
                  "properties": {
                    "PlayerId": {
                      "type": "string"
                    },
                    "Hand": {
                      "type": "array",
                      "items": [
                        {
                          "type": "object",
                          "properties": {
                            "QueryKey": {
                              "type": "string"
                            },
                            "GameClassId": {
                              "type": "integer"
                            },
                            "Id": {
                              "type": "integer"
                            }
                          },
                          "required": [
                            "QueryKey",
                            "GameClassId",
                            "Id"
                          ]
                        }
                      ]
                    },
                    "DrawPile": {
                      "type": "array",
                      "items": [
                        {
                          "type": "object",
                          "properties": {
                            "QueryKey": {
                              "type": "string"
                            },
                            "GameClassId": {
                              "type": "integer"
                            },
                            "Id": {
                              "type": "integer"
                            }
                          },
                          "required": [
                            "QueryKey",
                            "GameClassId",
                            "Id"
                          ]
                        }
                      ]
                    },
                    "DiscardPile": {
                      "type": "array",
                      "items": [
                        {
                          "type": "object",
                          "properties": {
                            "QueryKey": {
                              "type": "string"
                            },
                            "GameClassId": {
                              "type": "integer"
                            },
                            "Id": {
                              "type": "integer"
                            }
                          },
                          "required": [
                            "QueryKey",
                            "GameClassId",
                            "Id"
                          ]
                        }
                      ]
                    }
                  },
                  "required": [
                    "PlayerId",
                    "Hand",
                    "DrawPile",
                    "DiscardPile"
                  ]
                }
              ]
            }
          },
          "required": [
            "MonsterMaxLife",
            "MonsterLife",
            "MonsterGameClassId",
            "PlayersInfo"
          ]
        },
        "PlayersInfo": {
          "type": "array",
          "items": [
            {
              "type": "object",
              "properties": {
                "ActionsFinished": {
                  "type": "boolean"
                },
                "PlayerId": {
                  "type": "string"
                },
                "GameClassId": {
                  "type": "integer"
                },
                "MaxLife": {
                  "type": "integer"
                },
                "Life": {
                  "type": "integer"
                },
                "CurrentBlock": {
                  "type": "integer"
                },
                "Cards": {
                  "type": "array",
                  "items": [
                    {
                      "type": "object",
                      "properties": {
                        "QueryKey": {
                          "type": "string"
                        },
                        "GameClassId": {
                          "type": "integer"
                        },
                        "Id": {
                          "type": "integer"
                        }
                      },
                      "required": [
                        "QueryKey",
                        "GameClassId",
                        "Id"
                      ]
                    }
                  ]
                }
              },
              "required": [
                "ActionsFinished",
                "PlayerId",
                "GameClassId",
                "MaxLife",
                "Life",
                "CurrentBlock",
                "Cards"
              ]
            }
          ]
        }
      },
      "required": [
        "CurrentLevel",
        "EncounterInfo",
        "PlayersInfo"
      ]
    },
    "State": {
      "type": "integer"
    },
    "PartitionKey": {
      "type": "string"
    },
    "Id": {
      "type": "string"
    }
  },
  "required": [
    "IsInviteOnly",
    "Name",
    "AdminId",
    "Players",
    "GameInfo",
    "State",
    "PartitionKey",
    "Id"
  ]
}
```

### Documento do Player

```json
{
  "$schema": "http://json-schema.org/draft-04/schema#",
  "type": "object",
  "properties": {
    "Id": {
      "type": "string"
    },
    "CurrentRoom": {
      "type": "string"
    },
    "Discriminator": {
      "type": "string"
    },
    "Email": {
      "type": "string"
    },
    "Name": {
      "type": "string"
    },
    "PartitionKey": {
      "type": "string"
    },
    "PasswordHash": {
      "type": "array",
      "items": [
        {
          "type": "integer"
        }
      ]
    },
    "PasswordSalt": {
      "type": "array",
      "items": [
        {
          "type": "integer"
        }
      ]
    },
    "Progrees": {
      "type": "integer"
    },
    "Achivements": {
      "type": "array",
      "items": [
        {
          "type": "object",
          "properties": {
            "AchievmentKey": {
              "type": "integer"
            },
            "UnlockedAt": {
              "type": "string"
            }
          },
          "required": [
            "AchievmentKey",
            "UnlockedAt"
          ]
        }
      ]
    }
  },
  "required": [
    "Id",
    "CurrentRoom",
    "Discriminator",
    "Email",
    "Name",
    "PartitionKey",
    "PasswordHash",
    "PasswordSalt",
    "Progrees",
    "Achivements"
  ]
}
```

## Modelo Físico

Entregar um arquivo banco.sql contendo os scripts de criação das tabelas do banco de dados. Este arquivo deverá ser incluído dentro da pasta src\bd.

## Tecnologias Utilizadas

<!-- Tabela gerada apartir do arquivo: ./img/Tecnologias_Usadas.tgn -->
| **Contexto de uso** | **Tecnologias Usadas**                            |
|---------------------|---------------------------------------------------|
| **Hospedagem**      | Azure Cloud                                       |
| **Frontend Web**    | React, Axios, StyledComponents e React-router-dom |
| **Backend**         | C#, Asp.Net, Azure Functions e SignalR.           |
| **Frontend Mobile** | React Native, React Navigation, SignalR e axios   |

## Hospedagem

Utilizamos o Microsoft Azure como nosso provedor de hospedagem, que não apenas oferece uma infraestrutura escalável e confiável, mas também suporta uma arquitetura serverless. O Azure permite que implantemos, gerenciemos e dimensionemos nosso projeto de maneira eficaz e eficiente.

Optar por uma arquitetura serverless traz várias vantagens. Primeiramente, ela permite uma escalabilidade automática. Isso significa que o Azure gerencia a alocação de recursos para nossa aplicação com base na demanda, eliminando a necessidade de provisionar e gerenciar servidores.

Em segundo lugar, a arquitetura serverless segue o modelo de pagamento pelo uso, o que significa que só pagamos pelos recursos de computação que realmente usamos. Isso pode levar a uma redução significativa nos custos operacionais.

Além disso, a arquitetura serverless permite que nos concentremos mais no desenvolvimento do aplicativo, pois a manutenção e a administração do servidor são tratadas pelo Azure. Isso resulta em um tempo de colocação no mercado mais rápido.

A escolha do Azure como nosso provedor de hospedagem garante que nossa aplicação seja hospedada em um ambiente seguro e de alto desempenho. Além disso, a natureza serverless do Azure permite uma alta disponibilidade e tolerância a falhas, proporcionando uma experiência estável e confiável aos nossos usuários.

## Qualidade de Software

A tabela a seguir mostra característica de qualidade de software (como descritas na ISO/IEC 25010), selecionadas por nortearem o desenvolvimento dessa aplicação, além de seu conceito, justificativa, e metricas avaliativas.

| Característica                       | Conceito                                 | Justificativa                                                                                                                              | Principais métrica                                                                            |
|--------------------------------------|------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------|
| Manutenibilidade<br>Testabilidade    | É fácil testar quando se faz alterações? | Foi escolhida afim de manter o desenvolvimento agil e gradual, <br>porem garantindo que o que foi adicionado é retroativamente compativel. | - Numero de linhas cobertas por testes unitários<br>- Numero de dependências por caso de uso |
| Manutenibilidade<br>Modificabilidade | É fácil modificar e remover defeitos?    | Foi escolhida afim de agilizar o desenvolvimento.                                                                                          | - Numero de linhas mínimas requeridas para um novo caso de uso                               |

## Justificativa da utilização de um banco de dados Nosql

Eles oferecem escalabilidade horizontal eficiente, flexibilidade de esquema, alta disponibilidade e tolerância a falhas. Além disso, são otimizados para alto desempenho, oferecem modelos de dados específicos e integram-se bem com tecnologias modernas, tudo isso com custos de manutenção geralmente mais baixos do que os bancos de dados relacionais. Essas características os tornam essenciais para suportar a crescente demanda de armazenamento e acesso a dados em ambientes distribuídos, garantindo uma experiência de usuário responsiva e eficiente. 
