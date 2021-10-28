#ifndef _SOLO_QUERY_H
#define _SOLO_QUERY_H "Query"
#include <iostream>
namespace solo {
  /*
   * Query returns a snapshot document which can viewed as a
   * proxy object.
   * The query can include regex
   * Query must be executed by the Network class and the query
   * must be posted and packaged by SOLO
   */
class Query {
public:
  std::string query;
  std::string op;
  std::string field;
  std::string value;
  Query();
  Query(std::string);
  void execute();
};
}
#endif
