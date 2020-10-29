import last_yandere as last
import random_yandere as random
import telebot
import logging
import config
import Token as Tg
import sqlalchemy as db


bot = telebot.TeleBot(Tg.token)


engine = db.create_engine('sqlite:///db.sqlite')
connection = engine.connect()
metadata = db.MetaData()

main = db.Table('main', metadata,
    db.Column('Id', db.Integer(), primary_key=True),
    db.Column('Nickname', db.String(255), nullable=False),
    )

metadata.create_all(engine)


def add_new_user(message):
    try:
        connection = engine.connect()
        query = db.insert(main).values(Id=message.from_user.id, Nickname=message.from_user.username) 
        ResultProxy = connection.execute(query)
    except:
        logging.warning('Юзер есть в БД | ' + message.text)


def check_db():
    connection = engine.connect()
    results = connection.execute(db.select([main]))
    for row in results:
        print(row)


@bot.message_handler(commands=['start'])
def start_message(message):
    bot.send_message(message.chat.id, 'Привет, ' + message.from_user.username + ', ты написал мне /start')
    add_new_user(message)
    #check_db()
    logging.info(message.from_user.username + ' | ' + '/start')


@bot.message_handler(commands=['last'])
def last_message(message):
    last.last_art(message)


@bot.message_handler(commands=['random'])
def random_message(message):
    random.random_art(message)


bot.polling()
