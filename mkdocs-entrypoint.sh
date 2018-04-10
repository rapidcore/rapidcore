#!/bin/sh

cd /app/repository

function mkdocsServe {
    mkdocs serve --dev-addr "0.0.0.0:8000"
}

case "$1" in
    build)
        mkdocs build
        exit;;
    serve)
        mkdocsServe
        ;;
    *)
        mkdocsServe
        ;;
esac
