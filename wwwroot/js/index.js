window.onload = function() {
    // בדוק אם יש טוקן בקוקיז
    if (token) {
        // אם יש טוקן, הפנה לדף המשתמשים
        window.location.href = '/author.html';
    } else {
        // אם אין טוקן, תוכל להפנות לדף הלוגין או להשאיר את העמוד הנוכחי
        window.location.href = '/Login';
    }
};
