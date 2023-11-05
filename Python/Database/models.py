from sqlalchemy import BigInteger, Column, ForeignKey, Integer, SmallInteger, String, Table, UniqueConstraint, DateTime
from sqlalchemy.orm import relationship
from sqlalchemy.ext.declarative import declarative_base

Base = declarative_base()
metadata = Base.metadata

class User(Base):
    __tablename__ = 'user'
    __table_args__ = (
        UniqueConstraint("name"),
    )

    user_id = Column(BigInteger, primary_key=True, autoincrement=True)
    name = Column(String(255), nullable=False)
    passHash = Column(String(255), nullable=True)


class UserLocation(Base):
    __tablename__ = 'user_location'

    user_id = Column(ForeignKey('user.user_id'), primary_key=True, nullable=False)
    geo_hash = Column(String(255), nullable=False)


class Interaction(Base):
    __tablename__ = 'interaction'
    __table_args__ = (
        UniqueConstraint("interaction_id"),
    )

    interaction_id = Column(BigInteger, primary_key=True, autoincrement=True)
    interaction_time = Column(DateTime, nullable=False)
    primary_user_id = Column(ForeignKey('user.user_id'), nullable=False)
    interacted_with_user_id = Column(ForeignKey('user.user_id'), nullable=False)
    interacted_at_hash = Column(String(255), nullable=False)


class ScrapBook(Base):
    __tablename__ = 'scrapbook'

    interaction_id = Column(ForeignKey('interaction.interaction_id'), nullable=False, primary_key=True)
    data = Column(String(255))


class Friend(Base):
    __tablename__ = 'friend'

    primary_user_id = Column(ForeignKey('user.user_id'), nullable=False, primary_key=True)
    friend_user_id = Column(ForeignKey('user.user_id'), nullable=False, primary_key=True)
    interaction_id = Column(ForeignKey('interaction.interaction_id'), nullable=False)


class LandMark(Base):
    __tablename__ = 'landmark'

    landmark_id = Column(BigInteger, primary_key=True, autoincrement=True)
    geo_hash = Column(String(255))
    display_name = Column(String(255))
