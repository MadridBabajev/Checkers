using Domain.Db;

namespace WebUIHandler;

// ReSharper disable once InconsistentNaming
public static class WebUIBoardHandler
{
    
    public static string CreateFrontEndBoard(List<CheckersPiece> gameBoard,
                                            short height,
                                            short width)
    {

        // TODO you will probably have to calculate those based on the window size in a separate method
        // with the width of 8 and laptop screen size .55 rem/tile seems to fit perfectly 
        var sizeCoefficient = width * 0.55;
        // var rowCoefficient = height * 0.55;
        var firstCycle = true;
        var lastWasWhiteTile = true;

        var retString = "";
        var currentContainer = 
            $"<div class='d-flex flex-row' style='min-height: {sizeCoefficient}rem; width: fit-content;'>";
        for (short i = 0; i <= height; i++)
        {
            for (short j = 0; j <= width - 1; j++)
            {
                if (j % 8 == 0)
                {
                    
                    if (!firstCycle)
                    {
                        retString += currentContainer + "</div>";
                        currentContainer =
                            $"<div class='d-flex flex-row' style='min-height: {sizeCoefficient}rem; width: fit-content;'>";
                        lastWasWhiteTile = !lastWasWhiteTile;
                    }
                    
                    firstCycle = false;
                    
                }

                if (lastWasWhiteTile)
                {
                    currentContainer += GetTileString(sizeCoefficient, lastWasWhiteTile, j, i);
                }
                else
                {
                    
                    var pieceWasFound = false;
                    foreach (var piece in gameBoard)
                    {
                        if (piece.XCoordinate != j || piece.YCoordinate != i) continue;
                        currentContainer += GetTileStringWithAPiece(piece, sizeCoefficient, j, i);
                        pieceWasFound = true;
                        break;
                    }
                    if (!pieceWasFound) currentContainer += GetTileString(sizeCoefficient, lastWasWhiteTile, j, i);
                }

                lastWasWhiteTile = !lastWasWhiteTile;
            }
        }

        return retString;
    }

    private static string GetTileString(double sizeCoefficient, bool whiteTile, short x, short y) =>
                                    $"<div class='checkersTile {(!whiteTile ? "black-tile" : "")} border border-1 border-dark' " +
                                    $"data-coordinates='{x}:{y}' data-has_piece='0'" +
                                    $"style='max-height: {sizeCoefficient}rem; " +
                                    $"width: {sizeCoefficient}rem !important; " +
                                    $"background-color: {(whiteTile ? "#FDFEFE" : "#2980B9")}'>" +
                                    "</div>";

    private static string GetTileStringWithAPiece(CheckersPiece piece, double sizeCoefficient, short x, short y)
    {
        var pieceIconStr = !piece.IsQueen ? $"<svg id='Layer_2' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' x='0px' y='0px' viewBox='0 0 512 512' class='checkers-piece-icon-board {(piece.Color == EPieceColor.White ? "white-piece" : "")}' xml:space='preserve'><g><g><g><path d='M256,80.574C159.27,80.574,80.574,159.27,80.574,256S159.27,431.426,256,431.426S431.426,352.73,431.426,256 S352.73,80.574,256,80.574z M256,411.028c-85.483,0-155.028-69.545-155.028-155.028S170.517,100.972,256,100.972 S411.028,170.517,411.028,256S341.483,411.028,256,411.028z'/> <path d='M437.02,74.98C388.667,26.628,324.38,0,256,0S123.333,26.628,74.98,74.98C26.628,123.333,0,187.62,0,256 s26.628,132.667,74.98,181.02C123.333,485.372,187.62,512,256,512s132.667-26.628,181.02-74.98C485.372,388.667,512,324.38,512,256S485.372,123.333,437.02,74.98z M256,491.602c-129.911,0-235.602-105.69-235.602-235.602 S126.089,20.398,256,20.398S491.602,126.089,491.602,256S385.911,491.602,256,491.602z'/></g></g></g> <g><g><path d='M378.805,428.055c-3.052-4.733-9.363-6.098-14.098-3.047c-32.392,20.879-69.982,31.915-108.706,31.915 c-5.633,0-10.199,4.566-10.199,10.199c0,5.633,4.566,10.199,10.199,10.199c42.652,0,84.064-12.161,119.758-35.169 C380.492,439.102,381.856,432.79,378.805,428.055z'/></g></g> <g><g><path d='M413.816,396.61c-4.02-3.945-10.477-3.886-14.424,0.134c-2.113,2.153-4.311,4.294-6.535,6.363 c-4.122,3.838-4.353,10.292-0.515,14.415c2.009,2.158,4.734,3.249,7.468,3.249c2.489,0,4.983-0.905,6.947-2.733 c2.446-2.277,4.866-4.633,7.192-7.004C417.896,407.013,417.836,400.556,413.816,396.61z'/></g>" +
                                            "</g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g></svg>" 
            : $"<svg class='checkers-piece-icon-board-queen {(piece.Color == EPieceColor.White ? "white-piece" : "")}' viewBox='0 0 24 24' xmlns='http://www.w3.org/2000/svg'><path d='M12 21.75C10.0716 21.75 8.18657 21.1782 6.58319 20.1068C4.97982 19.0355 3.73013 17.5127 2.99218 15.7312C2.25422 13.9496 2.06114 11.9892 2.43735 10.0979C2.81355 8.20656 3.74215 6.46928 5.10571 5.10571C6.46928 3.74215 8.20656 2.81355 10.0979 2.43735C11.9892 2.06114 13.9496 2.25422 15.7312 2.99218C17.5127 3.73013 19.0355 4.97982 20.1068 6.58319C21.1782 8.18657 21.75 10.0716 21.75 12C21.7474 14.5851 20.7193 17.0635 18.8914 18.8914C17.0635 20.7193 14.5851 21.7474 12 21.75ZM12 3.75C10.3683 3.75 8.77326 4.23386 7.41655 5.14038C6.05984 6.0469 5.00242 7.33538 4.378 8.84287C3.75358 10.3504 3.5902 12.0092 3.90853 13.6095C4.22685 15.2098 5.01259 16.6799 6.16637 17.8336C7.32016 18.9874 8.79017 19.7732 10.3905 20.0915C11.9909 20.4098 13.6497 20.2464 15.1571 19.622C16.6646 18.9976 17.9531 17.9402 18.8596 16.5835C19.7661 15.2268 20.25 13.6317 20.25 12C20.2474 9.81278 19.3773 7.7159 17.8307 6.1693C16.2841 4.62269 14.1872 3.75265 12 3.75Z'/><path d='M15 17.38C14.8776 17.3798 14.7573 17.3489 14.65 17.29L12 15.89L9.34 17.29C9.21584 17.3548 9.07604 17.3834 8.93643 17.3728C8.79681 17.3622 8.66295 17.3128 8.55 17.23C8.43878 17.1467 8.35217 17.0349 8.29936 16.9064C8.24654 16.7779 8.22948 16.6374 8.25 16.5L8.76 13.5L6.61 11.45C6.51051 11.3516 6.44012 11.2276 6.40659 11.0918C6.37307 10.9559 6.37771 10.8134 6.42 10.68C6.46132 10.5504 6.53713 10.4345 6.6393 10.3446C6.74147 10.2548 6.86615 10.1944 7 10.17L10 9.73001L11.33 7.00001C11.3992 6.88398 11.4973 6.78791 11.6147 6.72118C11.7322 6.65446 11.8649 6.61938 12 6.61938C12.1351 6.61938 12.2678 6.65446 12.3853 6.72118C12.5027 6.78791 12.6008 6.88398 12.67 7.00001L14 9.73001L17 10.17C17.1393 10.1893 17.2703 10.2473 17.3781 10.3375C17.486 10.4277 17.5663 10.5464 17.61 10.68C17.6523 10.8134 17.6569 10.9559 17.6234 11.0918C17.5899 11.2276 17.5195 11.3516 17.42 11.45L15.27 13.54L15.78 16.54C15.8005 16.6774 15.7835 16.8179 15.7306 16.9464C15.6778 17.0749 15.5912 17.1867 15.48 17.27C15.3376 17.3612 15.1679 17.4 15 17.38ZM12 14.29C12.1224 14.2902 12.2427 14.3212 12.35 14.38L14.01 15.25L13.69 13.41C13.6689 13.2889 13.678 13.1644 13.7163 13.0475C13.7547 12.9307 13.8212 12.8251 13.91 12.74L15.25 11.43L13.4 11.16C13.2785 11.1442 13.1628 11.0985 13.0633 11.0269C12.9638 10.9554 12.8836 10.8602 12.83 10.75L12 9.07001L11.17 10.75C11.1164 10.8602 11.0362 10.9554 10.9367 11.0269C10.8372 11.0985 10.7215 11.1442 10.6 11.16L8.75 11.43L10.09 12.74C10.1788 12.8251 10.2453 12.9307 10.2837 13.0475C10.322 13.1644 10.3311 13.2889 10.31 13.41L10 15.25L11.66 14.38C11.7643 14.3227 11.881 14.2919 12 14.29Z' /></svg>";
        var retString = "<div class='checkersTile black-tile border border-1 border-dark d-flex justify-content-center' " +
                        $"data-coordinates='{x}:{y}' data-has_piece='1'" +
                        $"style='max-height: {sizeCoefficient}rem; " +
                        $"width: {sizeCoefficient}rem !important; " +
                        "background-color: #2980B9';>" +
                        pieceIconStr + "</div>";
        // $"<svg id='Layer_2' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' x='0px' y='0px' viewBox='0 0 512 512' class='checkers-piece-icon-board {(piece.Color == EPieceColor.White ? "white-piece" : "")}' @* style='enable-background:new 0 0 512 512;' *@ xml:space='preserve'><g><g><g><path d='M256,80.574C159.27,80.574,80.574,159.27,80.574,256S159.27,431.426,256,431.426S431.426,352.73,431.426,256 S352.73,80.574,256,80.574z M256,411.028c-85.483,0-155.028-69.545-155.028-155.028S170.517,100.972,256,100.972 S411.028,170.517,411.028,256S341.483,411.028,256,411.028z'/> <path d='M437.02,74.98C388.667,26.628,324.38,0,256,0S123.333,26.628,74.98,74.98C26.628,123.333,0,187.62,0,256 s26.628,132.667,74.98,181.02C123.333,485.372,187.62,512,256,512s132.667-26.628,181.02-74.98C485.372,388.667,512,324.38,512,256S485.372,123.333,437.02,74.98z M256,491.602c-129.911,0-235.602-105.69-235.602-235.602 S126.089,20.398,256,20.398S491.602,126.089,491.602,256S385.911,491.602,256,491.602z'/></g></g></g> <g><g><path d='M378.805,428.055c-3.052-4.733-9.363-6.098-14.098-3.047c-32.392,20.879-69.982,31.915-108.706,31.915 c-5.633,0-10.199,4.566-10.199,10.199c0,5.633,4.566,10.199,10.199,10.199c42.652,0,84.064-12.161,119.758-35.169 C380.492,439.102,381.856,432.79,378.805,428.055z'/></g></g> <g><g><path d='M413.816,396.61c-4.02-3.945-10.477-3.886-14.424,0.134c-2.113,2.153-4.311,4.294-6.535,6.363 c-4.122,3.838-4.353,10.292-0.515,14.415c2.009,2.158,4.734,3.249,7.468,3.249c2.489,0,4.983-0.905,6.947-2.733 c2.446-2.277,4.866-4.633,7.192-7.004C417.896,407.013,417.836,400.556,413.816,396.61z'/></g>" +
                        // "</g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g></svg>" +
        
        return retString; //GetTileString(sizeCoefficient, false, piece.XCoordinate, piece.YCoordinate);
    }
}