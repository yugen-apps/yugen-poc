# docker-compose

> $env:VERSION="1.0"
> docker compose --file "./Docker/docker-compose.yml" build
> sudo docker compose build

> docker tag poc-workerservice:latest ghcr.io/yugen-apps/poc-workerservice:latest
> docker tag poc-workerservice:latest ghcr.io/yugen-apps/poc-workerservice:1.0

> sudo docker image ls
> sudo docker image prune

> docker compose --file "./Docker/docker-compose.yml" up

> sudo docker container ls --all
> sudo docker container stop poc-workerservice
> sudo docker container prune

> docker push ghcr.io/yugen-apps/poc-workerservice:latest
> docker push ghcr.io/yugen-apps/poc-workerservice:v1.0


>  docker run -it poc-workerservice

>  docker pull ghcr.io/yugen-apps/poc-workerservice:latest