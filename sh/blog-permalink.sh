
dirname=$(date "+%d%m%Y")

mkdir -p "${dirname}"

touch "${dirname}/$(shasum ${BLOG_INPUT_FILE}" | cut -d' ' -f1)"

case $1 in
    publish) echo "Push to git, git commit -m $(uname -a)" ;;
    "") echo "Dfault action" ;;
    *) echo "Unknown" ;;
esac
