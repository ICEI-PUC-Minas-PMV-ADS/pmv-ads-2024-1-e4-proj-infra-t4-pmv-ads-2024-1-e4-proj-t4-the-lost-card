# The Lost Cards

`Análise e desenvolvimento de sistemas`

`Projeto: Desenvolvimento de uma Aplicação Distribuída`

`2024/1`

The Lost Cards é mais do que apenas um jogo de cartas single player - é uma jornada estratégica e emocionante que combina a cadência e a estratégia dos tradicionais jogos de cartas colecionáveis com um toque único: a construção do seu deck acontece durante a própria jornada. Seu objetivo? Proporcionar diversão e desafio a cada partida, enquanto cada nova carta conquistada molda e redefine sua estratégia. Em The Lost Cards, cada jogo é uma experiência única, onde a emoção e a surpresa se fundem para criar uma aventura cativante e inesquecível.

## Integrantes

* Adeilton Rodrigues Farias Junior
* Carlos Alberto Mendonça Vasconselos
* Gustavo Henrique de Jesus Almeida
* Luiz Eduardo de Jesus Santana
* Pedro Rafael da Cruz Almeida

## Orientador

* Pedro Alves de Oliveira

## Instruções de utilização

### Abra o terminal e clone o repositório:

```shell
git clone https://github.com/ICEI-PUC-Minas-PMV-ADS/pmv-ads-2024-1-e4-proj-infra-t4-pmv-ads-2024-1-e4-proj-t4-the-lost-card.git

cd ./pmv-ads-2024-1-e4-proj-infra-t4-pmv-ads-2024-1-e4-proj-t4-the-lost-card/src
```

#### executar o backend:

Instale o [dotnet na versão 6](https://dotnet.microsoft.com/pt-br/download/dotnet/6.0)
Instale o [Azure Functions Core Tools](https://learn.microsoft.com/pt-br/azure/azure-functions/create-first-function-cli-csharp?tabs=windows%2Cazure-cli)

```shell
cd ./backend/Presentation

dotnet restore
func start
```

#### executar o frontendWeb:

Instale o [node na versão 18](https://nodejs.org/pt/blog/release/v18.12.1)

```shell
cd ./src/frontend-web

npm i
npm run dev
```

#### executar o frontendMobile

Instale o [node na versão 18](https://nodejs.org/pt/blog/release/v18.12.1)
Instale o [Platform tools (Adb)](https://developer.android.com/tools/releases/platform-tools)
Instale o [Jdk 17](https://www.oracle.com/br/java/technologies/downloads/#java17)

Ative o modo densenvolvedor no celular e ligue a depuração USB.
Conecte o celular na porta USB do computador.

```shell
cd ./src/frontendMobile

npm i
adb reverse tcp:8081 tcp:8081
adb devices
```

Após a execução do ultimo comando você pode visualizar os hashes que identificam seu dispositivo conectado ao computador, use ele no próximo comando para identificar em qual celular instalar a aplicação.

```shell
# cole o hashIdentificador sem as chaves
npx react-native run-android - mode debug - deviceId {hashIdentificador}
```
# Documentação

<ol>
<li><a href="docs/01-Documentação de Contexto.md"> Documentação de Contexto</a></li>
<li><a href="docs/02-Especificação do Projeto.md"> Especificação do Projeto</a></li>
<li><a href="docs/03-Metodologia.md"> Metodologia</a></li>
<li><a href="docs/04-Projeto de Interface.md"> Projeto de Interface</a></li>
<li><a href="docs/05-Arquitetura da Solução.md"> Arquitetura da Solução</a></li>
<li><a href="docs/06-Template Padrão da Aplicação.md"> Template Padrão da Aplicação</a></li>
<li><a href="docs/07-Programação de Funcionalidades.md"> Programação de Funcionalidades</a></li>
<li><a href="docs/08-Plano de Testes de Software.md"> Plano de Testes de Software</a></li>
<li><a href="docs/09-Registro de Testes de Software.md"> Registro de Testes de Software</a></li>
<li><a href="docs/10-Plano de Testes de Usabilidade.md"> Plano de Testes de Usabilidade</a></li>
<li><a href="docs/11-Registro de Testes de Usabilidade.md"> Registro de Testes de Usabilidade</a></li>
<li><a href="docs/12-Apresentação do Projeto.md"> Apresentação do Projeto</a></li>
<li><a href="docs/13-Referências.md"> Referências</a></li>
</ol>

# Código

<li><a href="src/README.md"> Código Fonte</a></li>

# Apresentação

<li><a href="presentation/README.md"> Apresentação da solução</a></li>
