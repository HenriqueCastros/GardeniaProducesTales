# Steps to Set Up heroku env

1. Fazer login com Heroku
    ```
    heroku login
    ```
1. Adicionar remote ao Heroku
    <pre>heroku git:remote -a <span style="color:orange">name</span></pre>
    Onde `name` e o nome do app do Heroku, no nosso caso gardeniapt.

1. Checar se remote foi add
    ```
    git remote -v
    ```

1. Dar o push em apenas o diretorio onde esta a api
    <pre>git subtree push --prefix <span style="color:orange">path/to/dir</span> heroku <span style="color:orange">brach_deploy</span></pre>
    Onde: 
    - `path/to/dir` -> o diretorio onde se encontra a API, no nosso caso `/api`
    - `branch_deploy` -> a branch que vamos deployar, no caso `main`