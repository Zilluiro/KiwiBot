import last_yandere as last
import random_yandere as random
import telebot
import logging
import config
import Token as Tg


bot = telebot.TeleBot(Tg.token)


@bot.message_handler(commands=['start'])
def start_message(message):
    bot.send_message(message.chat.id, 'Привет, ' + message.from_user.username + ', ты написал мне /start')
    logging.info(message.from_user.username + ' | ' + '/start')


@bot.message_handler(commands=['last'])
def last_message(message):
    last.last_art(message)


@bot.message_handler(commands=['random'])
def random_message(message):
    random.random_art(message)


bot.polling()
