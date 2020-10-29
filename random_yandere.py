import logging
import requests
import config
import Token as Tg
import telebot
bot = telebot.TeleBot(Tg.token)


def random_art(message):
    response = requests.get('https://yande.re/post.json?tags=order:random')
    json = response.json()
    if json[0]["tags"] == 'tagme' or len(json[0]["tags"].split()) < 3:
        bot.send_message(message.chat.id, 'Прости, но я не могу отправить арт в текущий момент, так как он не прошел '
                                          'постановку тегов. Это нужно мне, чтобы фильтровать 18+ контент')
        logging.warning(message.from_user.username + ' | ' + 'Недостаточно тегов' + ' /last')
    else:
        is_18 = False
        for tag in config.tags:
            if tag in json[0]["tags"].split():
                is_18 = True
                break

        if is_18:
            bot.send_message(message.chat.id, "Ага, 18+")
            logging.warning(message.from_user.username + ' | ' + '18+ арт | ' + message.text)
            logging.warning('Теги: ' + json[0]["tags"])
        else:
            try:
                tag_items = ""
                for tag_item in json[0]["tags"].split():
                    tag_items += '#'+tag_item+', '
                bot.send_photo(message.chat.id, json[0]["sample_url"], tag_items)
                logging.info(message.from_user.username + ' | ' + message.text)
                logging.info(json[0]["tags"])
            except Exception:
                bot.send_message(message.chat.id, "Чет тг не понрав")
                logging.error(message.from_user.username + ' | ' + message.text)
