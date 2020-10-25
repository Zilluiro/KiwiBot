import telebot
import requests
import logging


bot = telebot.TeleBot('Токен сюда')

tags = ["no_bra", "tits", "boobs", "anus", "pussy", "uncensored", "censored", "open_shirt", "trap", "yaoi", "cum",
        "penis", "milk", "pantsu", "sex"]


logging.basicConfig(format='%(asctime)s - %(levelname)s - %(message)s', level=logging.INFO)


@bot.message_handler(commands=['start'])
def start_message(message):
    bot.send_message(message.chat.id, 'Привет, ' + message.from_user.username + ', ты написал мне /start')
    logging.info(message.from_user.username + ' | ' + '/start')


@bot.message_handler(commands=['last'])
def photo_message(message):
    response = requests.get('https://yande.re/post.json?limit=1')
    json = response.json()

    is_18 = False
    for tag in tags:
        if tag in json[0]["tags"].split():
            is_18 = True
            break

    if is_18:
        print(json[0]["sample_url"])
        bot.send_message(message.chat.id, "Ага, 18+")
        logging.warning(message.from_user.username + ' | ' + '18+ арт' + ' /last')
        logging.warning('Теги: ' + json[0]["tags"])
    else:
        try:
            bot.send_photo(message.chat.id, json[0]["sample_url"])
            logging.info(message.from_user.username + ' | ' + '/last')
        except Exception:
            bot.send_message(message.chat.id, "Чет тг не понрав")


bot.polling()
