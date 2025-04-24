#TODO finish this script, can be useful later
#!/usr/bin/env bash
input=$1
output_prefix=$2

if [ -z "${input}" ]; then
    echo "Input is invalid"
    exit
fi

if [ -z "${output_prefix}" ]; then
    echo "output_prefix is not given, choosing default value, ${input}-"
fi


total_duration=$(~/converted $ exiftool check_once_and_upload/sai\ baba\ 60th\ birthday\ celebration.m4v  | grep "Media Duration" | cut -d : -f 2-4 | xargs)

ffmpeg -i converted/check_once_and_upload/sai\ baba\ 60th\ birthday\ celebration.m4v -vcodec copy -acodec copy -ss 00:00:00 -t 00:30:00 output-part1.m4v -vcodec copy -acodec copy -ss 00:30:00 -t 01:00:00 output-part2.m4v -vcodec copy -acodec copy -ss 01:00:00 -t 1:24:04 output-part3.m4v
