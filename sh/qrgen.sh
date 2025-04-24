#!/bin/bash

dir='./qr_out'
if [ -e $dir ]; then 
  #FIXME grammatical errors
  echo -e "The directory $dir already exist, do you want to overwrite?[y/N]\c"
  read desc
  if [ $desc = 'y' ]; then
    #FIXME alternative descision checking is recommended
    rm -rf $dir
    mkdir $dir
  fi
else mkdir $dir
fi


#TODO query items in block to reduce load
#query items from db
items=`sqlite3 prod.db "select id from prod"`
for i in $items; do
  #FIXME change accordingly, use 'qrencode --help' for more info
  qrencode -o $dir/$i\.png $i
done
