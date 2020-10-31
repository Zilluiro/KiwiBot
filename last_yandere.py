import logging
import requests
from telebot import types

import config
import Token as Tg
import telebot
bot = telebot.TeleBot(Tg.token)


def last_art(message):
    bot.send_chat_action(message.chat.id, 'upload_photo')
    response = requests.get('https://yande.re/post.json?limit=1')
    json = response.json()
    keyboard = types.InlineKeyboardMarkup()
    callback_button = types.InlineKeyboardButton(text="Показать 18+", callback_data=json[0]["id"])
    keyboard.add(callback_button)
    if json[0]["tags"] == 'tagme' or len(json[0]["tags"].split()) < 3:
        bot.send_message(message.chat.id, "Прости, но я не могу отправить арт в текущий момент, так как он не прошел "
                                          "постановку тегов. Это нужно мне, чтобы фильтровать 18+ контент", reply_markup=keyboard)
        logging.warning(message.from_user.username + ' | ' + 'Недостаточно тегов' + ' /last')
    else:
        is_18 = False
        for tag in config.tags:
            if tag in json[0]["tags"].split():
                is_18 = True
                break

        if is_18:
            bot.send_message(message.chat.id, "Ага, 18+", reply_markup=keyboard)
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
