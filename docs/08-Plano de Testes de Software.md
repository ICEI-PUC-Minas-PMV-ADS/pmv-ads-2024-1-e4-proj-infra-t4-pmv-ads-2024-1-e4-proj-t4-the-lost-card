# Plano de Testes de Software

## Cenário de Teste: Registro do Usuário

- **Objetivo:** Verificar se o usuário consegue fazer o registro no sistema caso ele ainda não seja registrado.

- **Descrição:** Testar a função de registro do usuário no sistema.

- **Grupo de Usuários:** Usuários que desejam utilizar o The Lost Cards.

### Cenario CT001 - Registro do Usuário
Quando não há jogador existente com o mesmo email
Ao registrar um jogador com tal email
O registro é finalizado com sucesso

### Cenario CT002 - Registro com Usuário existente 
Quando há jogador existente com o mesmo email
Ao registrar um jogador com tal email
O registro é finalizado com erro

## Cenário de Teste: Login do Usuário

- **Objetivo:** Assegurar que a funcionalidade de Login do usuário esteja operando adequadamente, possibilitando que os usuários entrem em suas contas usando credenciais corretas.

- **Descrição:** Avaliar o processo de autenticação do usuário, fornecendo detalhes de login válidos e confirmando se a entrada é permitida no sistema.

- **Grupo de Usuários:** Usuários cadastrados no The Lost Cards.

### Cenario CT003 - SignIn do Usuário
Quando há jogador existente com determinado email e determinada senha
Ao realizar o signin usando tal email e tal senha
O SignIn é finalizado com sucesso, possibilitando a autenticação em outros casos de uso

### Cenario CT004 - SignIn do Usuário com senha errada
Quando há jogador existente com determinado email e determinada senha
Ao realizar o signin usando tal email e senha distinta
O SignIn é finalizado com erro

### Cenario CT005 - SignIn do Usuário inexistente
Quando não há jogador existente com determinado email
Ao realizar o signin usando tal email e qualquer senha
O SignIn é finalizado com erro

## Cenário de Teste: Buscar Salas

- **Objetivo:** Assegurar que a funcionalidade de Buscar Salas esteja implementada corretamente, permitindo que os usuários localizem salas específicas utilizando critérios de busca requeridos.

- **Descrição:** Avaliar o recurso de busca de salas, inserindo critérios de busca válidos e verificando se as salas correspondentes são retornadas pelo sistema.

- **Grupo de Usuários:** Usuários que têm permissão para acessar e buscar salas no sistema.


## Cenário de Teste: Ver Perfil

- **Objetivo:** Garantir que a funcionalidade Visualizar o Perfil esteja executando corretamente, permitindo que os usuários vejam as informações do perfil de outros usuários ou do seu próprio perfil.

- **Descrição:** Avaliar o recurso de visualização de perfil, acessando a página de perfil de um usuário e verificando se todas as informações relevantes são exibidas corretamente pelo sistema.

- **Grupo de Usuários:** Usuários que têm permissão para visualizar perfis no sistema.


## Cenário de Teste: Convidar

- **Objetivo:** Garantir que a funcionalidade Convidar esteja operando corretamente, permitindo que os usuários convidem outros usuários para uma sala específica.

- **Descrição:** Avaliar o recurso de convite, selecionando um usuário e enviando um convite para ele se juntar a uma sala específica, e verificando se o convite é enviado e recebido corretamente.

- **Grupo de Usuários:** Usuários que têm permissão para convidar outros usuários para salas no sistema.

## Cenário de Teste: Aceitar o convite

- **Objetivo:** Verificar que a funcionalidade Aceitar Convite de Sala esteja funcionando corretamente, permitindo que os usuários aceitem convites para se juntar a uma sala específica.

- **Descrição:** Avaliar o recurso de aceitar convites de sala, recebendo um convite para se juntar a uma sala e aceitando-o, e verificando se o usuário é adicionado à sala corretamente.

- **Grupo de Usuários:** Usuários que têm permissão para aceitar convites para salas no sistema.

## Cenário de Teste: Entrar em salas

- **Objetivo:** Garantir que a funcionalidade Entrar em Salas esteja sendo operada corretamente, permitindo que os usuários entrem em salas que foram criadas por outros jogadores.

- **Descrição:** Avaliar o recurso de entrar em salas, selecionando uma sala aberta e entrando nela, e verificando se o usuário consegue acessar a sala corretamente.

- **Grupo de Usuários:** Usuários que têm permissão para entrar em salas abertas no sistema.

## Cenário de Teste: Jogar

- **Objetivo:** Garantir que a funcionalidade Jogar esteja funcionando corretamente, permitindo que os usuários participem de jogos nas salas em que estão presentes.

- **Descrição:** Avaliar o recurso de jogar, iniciando uma partida, e verificando se o jogo funciona corretamente e se as regras são aplicadas de forma adequada.

- **Grupo de Usuários:** Usuários que têm permissão para participar de jogos no sistema.

## Cenário de Teste: Criar Sala

- **Objetivo:** Garantir que a funcionalidade Criar Sala esteja executando corretamente, permitindo que os usuários criem suas próprias salas de jogo.

- **Descrição:**  Avaliar o recurso de criar salas, iniciando o processo de criação de uma nova sala e verificando se a sala é criada corretamente e aparece na lista de salas disponíveis.

- **Grupo de Usuários:** Usuários que têm permissão para criar salas no sistema.

## Ferramentas de Testes

- Modelagem dos cenarios de teste feita usando Cucumber-Gherkin
- Testes unitarios do backend executados usando XUnit e NSubstitute