
#include <lp.h>

class Session {
	string session_key;
public:
	Session(const char *session_key){
		//load_db(session_key);
		this->session_key = session_key;
	}
};

const char *gen_new_session_key(){
	return "rumber_code";
}

class DB {
private:
	int *ptr = NULL;
	static DB *instance;
	DB(){

	}
	//add ptr to db and other such stuff
public:
	static DB *get_instance(){
		if(instance == NULL){
			instance = new DB();
		}
		return instance;
	}
	void add_to_db(const char *key){

	}

	int load_from_db(const char *key){
		return 0;
	}
};

DB *DB::instance = NULL;

Session *get_sesion(Node *n){
	return new Session("sdlfksdfk");
}

int lp_init_session(Node *_n){
	if(std::string(_n->readln()) == LP_CON){
		if(_n->writeln(LP_ACK)){
			string control_str = _n->readln();
			if(control_str == "\n"){
				//no session_key, create one and add to db
				const char *uniq_session_key = gen_new_session_key();
				DB::get_instance()->add_to_db(uniq_session_key);
				return 0;
			} else {
				//if we did not receive a plain new line '\n', then we are assuming it is a session_key
				//load the session from db
				string session_key_prob = _n->readln();
				if(!DB::get_instance()->load_from_db(session_key_prob.c_str())){
					//session key not found
					_n->writeln(LP_DEN);
					return -1;
				}
				_n->writeln(LP_ACK);
				return 0;
			}
		}
	}
	return -1;
}

int lp_connect_session(Node* _n, const char *session_key){
	if(_n->writeln(LP_CON)){
		if(std::string(_n->readln()) == LP_ACK){
			return 0;
		}
	}
	return -1;
}

//safe to call no-op
