/*
 * lp.h
 *
 *  Created on: Jul 6, 2017
 */


/*
 *
The client can ask the following requests
+---------+---------------------------------------+
|  CODE   |              DESCRIPTION              |
+---------+---------------------------------------+
| CON_REQ | Requesting for connection             |
| BIN_SEN | Sending a binary file                 |
| TXT_SEN | Sending a text file                   |
| SEN_CMD | Request for available commands        |
| DIAG_ST | Start diagnosis with provided data    |
| DIAG_CA | cancel the current diagnosis          |
| SET_BUF | sets the buffer to the given value    |
| GET_BUF | gets the buffer value from the server |
+---------+---------------------------------------+

Server replies with the following
+---------+--------------------------------------------+
|  CODE   |                DESCRIPTION                 |
+---------+--------------------------------------------+
| REQ_ACK | The request for connection is acknowledged |
| BINA_OK | Binary file is received                    |
| BINA_ER | Binary file is not received                |
| TEXT_OK | Text file is received                      |
| TEXT_ER | Text file is not received                  |
| DIAG_OK | Diagnosis completed successfully           |
| DIAG_ER | Diagnosis failed                           |
+---------+--------------------------------------------+
 *
 */
#ifndef LP_H
#define LP_H "LP_H"

#include<iostream>
#include<vector>
#include<unistd.h>
#include<string>
#if defined(__linux__)
#include<sys/sendfile.h>
#endif

#ifdef ENABLE_CLOG
#include <clog/clog.h>
#endif
#include<Node/Node.hpp>

#define LP_CON "CON"
#define LP_ACK "ACK"
#define LP_DEN "DEN"
#define LP_AVL_CMD "ACO"
#define LP_CMD "CMD"
#define LP_BIN "BIN"

#define LP_MSG_SIZE 3

using namespace std;
using namespace node;

int lp_init_session(Node *);
int lp_connect_session(Node *, const char * = NULL);
int lp_cmd(Node *, const char *text);
int lp_send_binary(Node *, const char *binary);
int open_session(Node *, const char *session_string);
int close_session(Node *, const char *session_string);

#endif /* LP_H */
