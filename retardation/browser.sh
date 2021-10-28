#!/bin/sh

url="$1"

[ -z "${url}" ] && printf "Url not entered\n" && exit 1

curl "${url}" | pandoc -f html  -t markdown | nvim -R -c ':set filetype=markdown'
