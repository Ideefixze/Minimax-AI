# Minimax-AI

Minimax AI implementation for games.

## Basic Introduction
Let's imagine a board game for two players. This game contains tiles and pawns. There are also some rules, that determine the total score for each player.
Also, there are some legal moves that you can do each turn, like placing a pawn or moving it. To make things easier: let's assume we can only place pawns.
Positioning pawns on the board makes different evaluation of the score. For two-player game, difference of scores `score(p1)-score(p2)` can be useful in determining victory.

more than 0 is a victory for Player 1
==0 means draw
less than 0 is a victory for Player 2

## Minimizing Player, Maximizing Player

Now we can clearly see that the objective of a player 1 is to maximize the differnce of the scores, and objective of a player 2 is to minimize difference of the scores. Many tutorials use this example, but I find it hard to fit in game for three or four players. Rather we can say that each player should maximize their own score and minimize the score of other players. In zero-sum games, of course, increase of score for one player is a decrease of score for a second player.

## Board State

Before we set up for an adventure, we should always think that our board can be changed only by some **legal commands**. That's why I am going to use **Command Design Pattern**. Commands will be the only way to modify the board. These commands would be useful later and they make my implementation egible for future changes and adaptations.

Each command bring the board to the different state, and thus change the scores for each player. 
Player 1 without a Queen is in really bad spot, where Player 2 who killed that pawn for one Tower should be the winner in that state.

## Tree of States

If we create a tree from a starting board state, looking by any legal move we can make right now, and by looking how our enemy may respond... and how we can respond... and how enemy may respond... it should be fairly easy for AI to determine which initial move can potentially bring them the biggest gain (or the least loses) in score. Under assumption that enemy of an AI player will play the best move they can make. 

Because we used **Command Design Pattern**, we can easily hold potential moves as objects and build tree where one node is a **board state and it's evaluation** and edges to the child nodes are **possible moves** from that **board state** for a given player.

## Pseudocode

From wikipedia, two player version. Simple concept:

```
function minimax(node, depth, maximizingPlayer) is
    if depth = 0 or node is a terminal node then
        return the heuristic value of node
    if maximizingPlayer then
        value := −∞
        for each child of node do
            value := max(value, minimax(child, depth − 1, FALSE))
        return value
    else (* minimizing player *)
        value := +∞
        for each child of node do
            value := min(value, minimax(child, depth − 1, TRUE))
        return value
```
Remember that this function only returns the best score from the root node, and you have to make pre-recursion function to find coresponding move.
Here's my **core** pseudocode for many players and for game usage:

```
(* remember that players[] have to be sorted in player turn order *)

function get_ai_move(board, player, players[]) is
    best_move := null
    best_score := −∞
    foreach possible_move in board.possible_moves_for_player(player) do
        board_after_move := board.copy()
        board_after_move.execute_move(possible_move)
        score = minimax(board_after_move, 1, player, players[])
        if score >= best_score then
            best_score := score
            best_move := possible_move
            
    return best_move (* null means no move is possible *)
    
function minimax(board, depth, player, players[])
    if depth = DEPTH or node is a terminal node then
        return board.evaluate(player) - sum(board.evaluate_players_except(players, player))
    
    player_turn = depth%players.count + player.turn_order
        
    if player_turn==player.turn_order
        value := −∞
        foreach possible_move in board.possible_moves_for_player(player) do
            board_after_move := board.copy()
            board_after_move.execute_move(possible_move)
            value := max(value, minimax(board_after_move, depth, player, players[]))
        return value
    else (*move for potential minimizers*)
        value := +∞
        foreach possible_move in board.possible_moves_for_player(players[player.turn_order]) do
            board_after_move := board.copy()
            board_after_move.execute_move(possible_move)
            value := min(value, minimax(board_after_move, depth, player, players[]))
        return value
    
```

## It is cool, but very slow

Depending on the size of your board, number of potential moves and on tree depth, this may work differently. To be honest, it is really slow. Is there any way we can make it faster? There is an optimization techique called alpha-beta pruning. Great explanation found on wikipedia:

*It stops evaluating a move when at least one possibility has been found that proves the move to be worse than a previously examined move. Such moves need not be evaluated further. When applied to a standard minimax tree, it returns the same move as minimax would, but prunes away branches that cannot possibly influence the final decision.*

This modifies our `minimax` function to:

```
function minimax_alphabeta(board, depth, player, players[], alpha, beta)
    if depth = DEPTH or node is a terminal node then
        return board.evaluate(player) - sum(board.evaluate_players_except(players, player))
    
    player_turn = depth%players.count + player.turn_order
        
    if player_turn==player.turn_order
        value := −∞
        foreach possible_move in board.possible_moves_for_player(player) do
            board_after_move := board.copy()
            board_after_move.execute_move(possible_move)
            value := max(value, minimax(board_after_move, depth, player, players[]))
            alpha = max(value, alpha)
            if alpha >= beta then
                break
        return value
    else (*move for potential minimizers*)
        value := +∞
        foreach possible_move in board.possible_moves_for_player(players[player.turn_order]) do
            board_after_move := board.copy()
            board_after_move.execute_move(possible_move)
            value := min(value, minimax(board_after_move, depth, player, players[]))
            beta = min(value, beta)
            if beta <= alpha then
                break
        return value
    
```
where initial values is `alpha:=INT.Minimum`, `beta:=INT.Maximum`.
        
