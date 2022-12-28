This repo contains a reproduction of a bug where two events with the same name 
under different namespaces do not recreate projections accurately

The event is associated with the wrong aggregate type.

```
docker rm --force martendb-tests
docker run -itd --name martendb-tests --restart=unless-stopped -p:6666:5432 -e POSTGRES_PASSWORD=somepassword postgres

```