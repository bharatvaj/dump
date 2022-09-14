#!/bin/sh

read a

case $a in
	c)
	echo 'Play sound file c'
	;;
	*)
	echo 'lol default'
	;;
esac
