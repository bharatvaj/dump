#include <iostream>
#include <solo/SOLO.hpp>
#include <clog/clog.h>
#include <Dot/Dot.hpp>
#include <solo/SoloLooper.hpp>
#include <soloconfig.h>
#include <argtable3.h>

using namespace std;
using namespace solo;
using namespace dot;

struct arg_lit *help, *version;
struct arg_end *a_end;

static const char *TAG = "solo.cpp";

int start_solo(SoloLooper &soloLooper)
{
log_inf(TAG, "Solo v%s started...", SOLO_VERSION);
  SOLO *solo = new SOLO();
  return soloLooper.run();
}

int exit_handler(int sig)
{
  exit(EXIT_SUCCESS);
}

int main(int argc, char *argv[])
{
  //TODO register signals
  void *argtable[] = {
      help = arg_litn(NULL, "help", 0, 1, "print this message"),
      version = arg_litn(NULL, "version", 0, 1, "print solo version"),
      a_end = arg_end(20),
  };

  int exitcode = 0;
  char progname[] = "solo";

  int nerrors;
  nerrors = arg_parse(argc, argv, argtable);

  if (help->count > 0)
  {
    printf("Usage: %s", progname);
    arg_print_syntax(stdout, argtable, "\n");
    printf("Monitor solo enabled devices\n\n");
    arg_print_glossary(stdout, argtable, "  %-25s %s\n");
    exitcode = 0;
    return 0;
  }

  if (version->count > 0)
  {
    cout << SOLO_VERSION << endl;
    return 0;
  }

  if (nerrors > 0)
  {
    arg_print_errors(stdout, a_end, progname);
    printf("Try '%s --help' for more information.\n", progname);
    exitcode = 1;
    return 0;
  }
  SoloLooper &soloLooper = *(new SoloLooper());
  return start_solo(soloLooper);
}
