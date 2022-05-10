from flask import Flask, request, jsonify
import gpt2
app = Flask(__name__)

@app.route('/getmsg/', methods=['GET'])
def respond():
    # Retrieve the name from the url parameter /getmsg/?name=
    name = request.args.get("input", None)

    # For debugging
    print(f"Received: {name}")

    response = {}

    name = gpt2.generate_text(name)
    # Check if the user sent a name at all
    if not name:
        response["ERROR"] = "No input found. Please send a input."
    # Check if the user entered a number
    elif str(name).isdigit():
        response["ERROR"] = "The input can't be numeric. Please send a string."
    else:
        response["MESSAGE"] = f"{name}"

    # Return the response in json format
    return jsonify(response)

@app.route('/')
def index():
    # A welcome message to test our server
    return "<h1>Welcome to gardenia Produces Tales API!</h1>"


if __name__ == '__main__':
    # Threaded option to enable multiple instances for multiple user access support
    app.run(threaded=True, port=5000)