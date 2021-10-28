#!/bin/sh

# Generate the white background
convert -size 256x256 xc:white white.png

# Add '~' in the centre
convert white.png -gravity Center  -pointsize 90 -annotate 0 "~" white1.png
