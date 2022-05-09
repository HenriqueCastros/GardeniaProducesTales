# Steps to Set Up heroku env

1. Fazer login com Heroku
```
heroku login
```
1. Adicionar remote ao Heroku
```
heroku git:remote -a ${name}
```
Onde `name` e o nome do app do Heroku, no nosso caso gardeniapt.

1. Checar se remote foi add
```
git remote -v
```

1. Dar o push em apenas o diretorio onde esta a api
```
git subtree push --prefix ${path/to/dir} heroku ${brach_deploy}
```
Onde: 
 - `path/to/dir` -> o diretorio onde se encontra a API, no nosso caso `/api`
 - `branch_deploy` -> a branch que vamos deployar, no caso `main`