import telebot
import requests
import logging


bot = telebot.TeleBot('Токен сюда')

tags = ["no_bra", "tits", "boobs", "anus", "pussy", "uncensored", "censored", "open_shirt", "trap", "yaoi", "cum",
        "penis", "milk", "sex"]


logging.basicConfig(format='%(asctime)s - %(levelname)s - %(message)s', level=logging.INFO)


@bot.message_handler(commands=['start'])
def start_message(message):
    bot.send_message(message.chat.id, 'Привет, ' + message.from_user.username + ', ты написал мне /start')
    logging.info(message.from_user.username + ' | ' + '/start')


@bot.message_handler(commands=['last'])
def photo_message(message):
    response = requests.get('https://yande.re/post.json?limit=1')
    json = response.json()

    if json[0]["tags"] == 'tagme':
        bot.send_message(message.chat.id, 'Прости, но я не могу отправить арт в текущий момент, так как он не прошел '
                                          'постановку тегов. Это нужно мне, чтобы фильтровать 18+ контент')
        logging.warning(message.from_user.username + ' | ' + 'Арт с тегом tagme! Игнорирую...' + ' /last')
    else:
        is_18 = False
        for tag in tags:
            if tag in json[0]["tags"].split():
                is_18 = True
                break

        if is_18:
            bot.send_message(message.chat.id, "Ага, 18+")
            logging.warning(message.from_user.username + ' | ' + '18+ арт' + ' /last')
            logging.warning('Теги: ' + json[0]["tags"])
        else:
            try:
                tag_items = ""
                for tag_item in json[0]["tags"].split():
                    tag_items += '#'+tag_item+', '
                bot.send_photo(message.chat.id, json[0]["sample_url"], tag_items)
                logging.info(message.from_user.username + ' | ' + '/last')
                logging.info(json[0]["tags"])
            except Exception:
                bot.send_message(message.chat.id, "Чет тг не понрав")
                logging.error(message.from_user.username + ' | ' + '/last')



bot.polling()
