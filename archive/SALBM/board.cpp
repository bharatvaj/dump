#include<iostream>
#include<vector>

using namespace std;

#define MAX_SIZ 256

class Board {
	private:
		int pR, pC;
		int r, c;
		char board[MAX_SIZ][MAX_SIZ];
		vector<int> rolls;
		void winStub(vector<int> rolls){}
		void snakeStub(int tr, int ct, int sr, int sc){}
		void ladderStub(int tr, int ct, int sr, int sc){}
	public:
		void (*onWin)(vector<int>);
		void (*onSnake)(int tr, int ct, int sr, int sc);
		void (*onLadder)(int tr, int ct, int sr, int sc);
		void setDimension(int r, int c){
			this->r = r;
			this->c = c;
		}
		Board(int r, int c){
			setDimension(r, c);
			pR = r - 1;
			pC = c - 1;
			//TODO assigning stubs
			//onWin = winStub;
			//onSnake = snakeStub;
		}
		void setCell(int r, int c, char val){
			board[r][c] = val;
		}
		void roll(int rollVal){
			//update rolls
			rolls.push_back(rollVal);
			//TODO calculate pR and pC using rollVal, pR and pC
			pR = r - (rollVal % r) - 1;
			pC = rollVal % c;
			if(board[pR][pC] == 'S'){
				//calculate source and destination
				int dR = 0;
				int dC = 0;
				//TODO calculate destination
				onSnake(dR, dC, pR, pC);
			} else if(board[pR][pC] == 'L'){
				//calculate source and destination
				int dR = 0;
				int dC = 0;
				//TODO calculate destination
				onLadder(dR, dR, pR, pC);
			}
			if(pR == 0 && pC == 0){
				onWin(rolls);
			}
		}
		
};

void win(vector<int> rolls){
	for(int i = 0; i < rolls.size(); i++){
		cout << rolls[i] << " ";
	}
	cout << endl;
}




int main(int argc, char *argv[]){
	int r, c, i, j;
	//get r and c
	cin >> r >> c;
	//board creation
	Board b(r, c);
	//assign handlers
	b.onWin = win;
	//get vals for board
	for(i = 0; i < r; i++){
		for(j = 0; j < c; j++){
			char val = 0;
			cin >> val;
			b.setCell(r, c, val);
		}
	}

//get vals for dice rolling
	int rolls[100];
	i = 0;
	while(cin >> rolls[i++]);
	int rollLen = i;
//print dice roll sequence required
	for(i = 0; i < rollLen; i++){
		b.roll(rolls[i]);
	}
}
