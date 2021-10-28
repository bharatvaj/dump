#!/bin/bash

# If not running interactively, don't do anything
[[ $- != *i* ]] && return
source ~/.git-completion.bash
alias ls='ls --color=auto'
alias grep="grep --color=auto"

parse_git_branch(){
	git branch 2> /dev/null | sed -e '/^[^*]/d' -e 's/* \(.*\)/\1/'
}
export PS1="\[\033[33m\]\$(parse_git_branch)\[\033[00m\] \w \$ "
