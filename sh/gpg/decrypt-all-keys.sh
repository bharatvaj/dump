#!/bin/sh

name=`find . -name '*.gpg'`
for i in ${name[@]}; do
	output_file=$(echo "$i" | sed 's/.gpg//g')
	echo $output_file
	gpg -d $i > $output_file
done
